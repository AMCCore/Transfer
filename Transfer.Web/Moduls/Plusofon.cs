using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Transfer.Common.Settings;
using System.Text;
using Dadata.Model;

namespace Transfer.Web.Moduls;

public class Plusofon
{
    private readonly TransferSettings _transferSettings;

    public Plusofon(TransferSettings TransferSettings)
    {
        _transferSettings = TransferSettings;
    }

    public async Task FlashCallSend(string PhoneNumber, string Code)
    {
        using (var httpClient = new HttpClient())
        {
            var jsonRequest = JsonConvert.SerializeObject(new { phone  = PhoneNumber, pin = Code});
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_transferSettings.PlusofonBaseApiUrl}/api/v1/flash-call/send"))
            {
                requestMessage.Headers.Add("Client", _transferSettings.PlusofonClientId);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _transferSettings.PlusofonToken);
                requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                using (var response = await httpClient.SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Some problem with Plusofon FlashCall API Send");
                    }
                    var responseData = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
