using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TPSecurity.Infrastructure.Services.APIs;

public static class HttpClientHelper
{
    private static void Auth(HttpClient client, string authenticationString)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.ConnectionClose = true;
        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }

    public static async Task<T> HttpGet<T>(string authenticationString, string path)
    {
        HttpClient client = new HttpClient();
        Auth(client, authenticationString);

        HttpResponseMessage response = await client.GetAsync(path);

        if (!response.IsSuccessStatusCode) return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public static string GetBasicAuth(string user, string password)
    {
        return $"{user}:{password}";
    }

    public static string GetFullPath(string baseUrl, string version, string concept, string id = null)
    {
        string idExtension = !string.IsNullOrWhiteSpace(id) ? "/" + id : "";
        return $"{baseUrl}/{version}/{concept}{idExtension}";
    }
}
