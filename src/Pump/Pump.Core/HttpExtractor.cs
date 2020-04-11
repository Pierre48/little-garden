using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Ppl.Core.Extensions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using RocksDbSharp;
using System.IO;
using Pump.Core.Metrics;

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
        private ILogger<HttpExtractor> Logger { get; }
        public IMetricsServer Metrics { get; }

        readonly RocksDb db = null;
        public HttpExtractor(ILogger<HttpExtractor> logger, IMetricsServer metrics)
        {
            Logger = logger;
            Metrics = metrics;
            var options = new DbOptions().SetCreateIfMissing(true).EnableStatistics();
            db = RocksDb.Open(options, Environment.ExpandEnvironmentVariables(Path.Combine(Environment.CurrentDirectory, "httpCall")));
        }
        public async Task<string> GetHtml(string url, Param[] parameters = null)
        {
            parameters?.ForEach(p => url = url.Replace("{" + p.Key + "}", p.Value));
            while (true)
            {
                try
                {
                    Logger.LogDebug("Scrapping " + url + "...");

                    string result =  db.Get(url);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Metrics.Inc("pump_httpextractor_nbloadedfromcache",1);
                        Logger.LogDebug(url + " => Loaded from cache :):):)");
                        return result;
                    }

                    using (var client = new HttpClient())
                    {
                        Logger.LogDebug(url + " => Not found in cache");
                        var response = await client.GetAsync(url);
                        Logger.LogDebug(url + " => Http request done : " + response.StatusCode);
                        result = await response.Content.ReadAsStringAsync();
                        result = result.Replace("\n", "")
                        .Replace("\t", "")
                        .Replace("\\\"", "\"");
                        db.Put(url, result);
                        Metrics.Inc("pump_httpextractor_nbloadedfromhttp", 1);
                        Logger.LogDebug(url + " => Loaded from http");
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e.GetFullMessage());
                    Thread.Sleep(60000);
                }
            }
        }

        public async Task<Byte[]> GetBytes(string url, Param[] parameters = null)
        {
            parameters?.ForEach(p => url = url.Replace("{" + p.Key + "}", p.Value));
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsByteArrayAsync();
            }
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                db?.Dispose();
            }

            disposed = true;
        }
        bool disposed = false;
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
