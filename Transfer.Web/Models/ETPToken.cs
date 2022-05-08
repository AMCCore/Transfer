using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;

namespace Transfer.Web.Models;

/// <summary>
/// получение Bearer Token
/// </summary>
internal static class ETPToken
{
    private static readonly object syncroObject = new object();

    private static string token = null;
    private static DateTime? tokenUpdate = null;

    private static readonly string ConsumerKey = "kVYvqppYL0zXHbo8j92PXg1jKGsa";
    private static readonly string ConsumerSecret = "ACd_OP0sHN1boRX0FoSTnylCxDoa";
    private static readonly string AuthorizationBasicUrl = "https://papi.mos.ru/token";

    /// <summary>
    /// Bearer Token
    /// </summary>
    public static string Token
    {
        get
        {
            if (string.IsNullOrWhiteSpace(token) || !tokenUpdate.HasValue || tokenUpdate < DateTime.Now)
            {
                UpdateToken();
            }
            return token;
        }
    }

    private static void UpdateToken()
    {
        lock (syncroObject)
        {
            if (string.IsNullOrWhiteSpace(token) || !tokenUpdate.HasValue || tokenUpdate.Value < DateTime.Now)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), new Uri(AuthorizationBasicUrl)))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {Base64Encode()}");

                        request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                        var response = httpClient.SendAsync(request).Result;
                        var contents = response.Content.ReadAsStringAsync().Result;

                        JObject json = JObject.Parse(contents);

                        token = (string)json["access_token"];
                        tokenUpdate = new DateTime(Convert.ToInt64(json["expires_in"]));
                        if (tokenUpdate <= DateTime.Now)
                            tokenUpdate = DateTime.Now.AddMinutes(5);
                    }
                }
            }
        }
    }

    private static string Base64Encode()
    {
        var plainTextBytes = Encoding.UTF8.GetBytes($"{ConsumerKey}:{ConsumerSecret}");
        return Convert.ToBase64String(plainTextBytes);
    }

}
