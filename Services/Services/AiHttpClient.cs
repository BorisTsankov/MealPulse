using RestSharp;
using Services.Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AiHttpClient : IAiHttpClient
    {
        private readonly RestClient _client = new();

        public virtual Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return _client.ExecuteAsync(request);
        }
    }
}