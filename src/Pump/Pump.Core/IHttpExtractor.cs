using System.Threading.Tasks;

namespace Pump.Core
{
    public interface IHttpExtractor
    {
        Task<byte[]> GetBytes(string url, Param[] parameters = null);
        Task<string> GetHtml(string url, Param[] parameters = null);
    }
}