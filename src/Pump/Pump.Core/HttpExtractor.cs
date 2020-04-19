using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ppl.Core.Extensions;
using Pump.Core.Metrics;
using RocksDbSharp;

namespace Pump.Core
{
    public struct Param
    {
        public Param(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class HttpExtractor : IHttpExtractor, IDisposable
    {
        private readonly RocksDb _db;
        private bool _disposed;

        public HttpExtractor(ILogger<HttpExtractor> logger, IMetricsServer metrics)
        {
            Logger = logger;
            Metrics = metrics;
            var options = new DbOptions().SetCreateIfMissing().EnableStatistics();
            _db = RocksDb.Open(options,
                Environment.ExpandEnvironmentVariables(Path.Combine(Environment.CurrentDirectory, "httpCall")));
        }

        private ILogger<HttpExtractor> Logger { get; }
        public IMetricsServer Metrics { get; }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public async Task<string> GetHtml(string url, Param[] parameters = null)
        {
            parameters?.ForEach(p => url = url.Replace("{" + p.Key + "}", p.Value));
            while (true)
                try
                {
                    Logger.LogDebug("Scrapping " + url + "...");

                    var result = _db.Get(url);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Metrics.Inc("pump_httpextractor_nbloadedfromcache", 1);
                        Logger.LogDebug(url + " => Loaded from cache :):):)");
                        return result;
                    }

                    var handler = new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, cetChain, policyErrors) => true
                    };

                    using (var client = new HttpClient(handler))
                    {
                        Logger.LogDebug(url + " => Not found in cache");
                        var response = await client.GetAsync(url);
                        Logger.LogDebug(url + " => Http request done : " + response.StatusCode);
                        Metrics.Inc("pump_httpextractor_bytesloaded", response.Content.Headers.ContentLength??0);
                        result = await response.Content.ReadAsStringAsync();
                        result = result.Replace("\n", "")
                            .Replace("\t", "")
                            .Replace("\\\"", "\"");
                        _db.Put(url, result);
                        Metrics.Inc("pump_httpextractor_nbloadedfromhttp", 1);
                        Logger.LogDebug(url + " => Loaded from http");
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Metrics.Inc("pump_httpextractor_errors", 1);
                    Logger.LogError(e.GetFullMessage());
                    Thread.Sleep(60000);
                }
        }

        public async Task<byte[]> GetBytes(string url, Param[] parameters = null)
        {
            parameters?.ForEach(p => url = url.Replace("{" + p.Key + "}", p.Value));
            var key = Encoding.UTF8.GetBytes(url);
            while (true)
                try
                {
                    var result = _db.Get(key);
                    if (result != null)
                    {
                        Metrics.Inc("pump_httpextractor_nbfileloadedfromcache", 1);
                        Logger.LogDebug(url + " => Loaded from cache :):):)");
                        return result;
                    }

                    using (var client = new HttpClient())
                    {
                        Logger.LogDebug(url + " => Not found in cache");
                        var response = await client.GetAsync(url);
                        Logger.LogDebug(url + " => Http request done : " + response.StatusCode);
                        Metrics.Inc("pump_httpextractor_bytesloaded", response.Content.Headers.ContentLength??0);
                        Logger.LogDebug(url + " => Loaded from http");
                        result = await response.Content.ReadAsByteArrayAsync();
                        _db.Put(key, result);
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Metrics.Inc("pump_httpextractor_errors", 1);
                    Logger.LogError(e.GetFullMessage());
                    Thread.Sleep(60000);
                }
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing) _db?.Dispose();

            _disposed = true;
        }
    }
}