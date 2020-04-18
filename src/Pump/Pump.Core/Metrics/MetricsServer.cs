using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ppl.Core.Container;
using Ppl.Core.Extensions;
using Prometheus;

namespace Pump.Core.Metrics
{
    public class MetricsServer : IMetricsServer
    {
        private readonly Dictionary<string, IGauge> _counters = new Dictionary<string, IGauge>();

        public MetricsServer(ContainerParameters parameters)
        {
            Parameters = parameters;
        }

        public ContainerParameters Parameters { get; }

        public void Set(string name, double value)
        {
            GetCounter(name).Set(value);
        }

        public void Open()
        {
            var server = new MetricServer(Parameters.GetIntParameter("MetricsPort"));
            server.Start();

            Task.Run(() =>
            {
                _counters.Values.ForEach(x => x.Inc());
                Thread.Sleep(TimeSpan.FromSeconds(10)); //TODO To Configure
            });
        }

        public void Inc(string name, long modifiedCount)
        {
            GetCounter(name).Inc(modifiedCount);
        }

        private IGauge GetCounter(string name)
        {
            lock (_counters)
            {
                if (!_counters.ContainsKey(name))
                    _counters.Add(name, Prometheus.Metrics.CreateGauge(name, name));
            }

            return _counters[name];
        }
    }
}