namespace Pump.Core.Metrics
{
    public interface IMetricsServer
    {
        void Open();
        void Set(string name, double value);
        void Inc(string name, long modifiedCount);
    }
}