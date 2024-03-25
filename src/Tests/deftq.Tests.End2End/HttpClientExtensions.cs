using System.Net.Http.Headers;

namespace deftq.Tests.End2End
{
    public static class HttpClientExtensions
    {
        public static HttpClient SetToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
