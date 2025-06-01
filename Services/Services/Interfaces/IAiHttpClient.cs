using RestSharp;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IAiHttpClient
    {
        Task<RestResponse> ExecuteAsync(RestRequest request);
    }
}