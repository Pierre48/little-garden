using Prometheus;
using System;
using System.Collections.Generic;
using System.Threading;
using Ppl.Core.Extensions;
using System.Threading.Tasks;

namespace Pump.Core.Metrics
{
    public class MetricsServer : IMetricsServer
    {
        private Dictionary<string, IGauge> counters = new Dictionary<string, IGauge>();
        public void Set(string name, double value)
        {
            GetCounter(name).Set(value);
        }

        public async Task Open() 
        {
            var server = new Prometheus.MetricServer(hostname:"127.0.0.1", port: 9999);//TODO To Configure
            server.Start();

            await Task.Run(() =>
            {
                counters.Values.ForEach(x => x.Inc());
                Thread.Sleep(TimeSpan.FromSeconds(10));//TODO To Configure
            });
        }

        public void Inc(string name, long modifiedCount)
        {
            GetCounter(name).Inc(modifiedCount);
        }

        private IGauge GetCounter(string name)
        {
            lock (counters)
                if (!counters.ContainsKey(name))
                {
                    counters.Add(name, Prometheus.Metrics.CreateGauge(name, name));
                }
            return counters[name];
        }
    }
}
