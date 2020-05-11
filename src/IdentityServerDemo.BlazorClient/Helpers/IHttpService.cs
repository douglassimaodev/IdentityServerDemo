using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerDemo.BlazorClient.Helpers
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<T>> Get<T>(string url, bool includeToken = true);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, bool includeToken = true);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T data);

        Task<HttpClient> GetHttpClient(bool includeToken = true);
    }
}
