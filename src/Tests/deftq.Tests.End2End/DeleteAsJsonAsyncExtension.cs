using System.Text;
using System.Text.Json;

namespace deftq.Tests.End2End
{
    public static class DeleteAsJsonAsyncExtension
    {
        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var jsonRequest = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            jsonRequest.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return client.SendAsync(jsonRequest);
        }
    }
}
