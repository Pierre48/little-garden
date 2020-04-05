using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pump.Core.Metrics
{
    public interface IMetricsServer
    {
        Task Open();
        void Set(string name, double value);
        void Inc(string name, long modifiedCount);
    }
}
