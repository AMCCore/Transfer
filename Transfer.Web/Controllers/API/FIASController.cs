using Dadata;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Transfer.Web.Models;
using Transfer.Web.Models.Fias;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class FIASController : ControllerBase
{
    private const string BasicUrl = "https://papi.mos.ru/api/fias/7.0";

    private const string token = "221296ccd7fcf17178d405383d1b5ed9004e25ea";
    private const string secret = "276ef12486de64a6ad36929fc1b1c82356af9e63";

    /// <summary>
    /// поиск адреса в ФИАС (прокси класс)
    /// </summary>
    /// <param name="Query"></param>
    [HttpGet]
    public async Task<IActionResult> SearchHome(string Query)
    {
        var api = new SuggestClientAsync(token);
        var result = await api.SuggestAddress(Query);

        return new JsonResult(result);


        var req = new FIASRequest()
        {
            Query = Query,
            Count = 20,
            LocationsBoost = new FIASRequestLocation[1] { new FIASRequestLocation { Region = "Москва" } }
        };
        using (var httpClient = new HttpClient())
        {
            var jsonRequest = JsonConvert.SerializeObject(req);
            using (var requestMessage = CreateHttpRequestMessage(HttpMethod.Post, new Uri($"{BasicUrl}/searchHome"), jsonRequest))
            {
                using (var response = await httpClient.SendAsync(requestMessage))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(responseData);
                    return Ok(json);
                }
            }
        }
    }

    /// <summary>
    ///     Формирование запроса
    /// </summary>
    /// <returns></returns>
    private static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, Uri url, string requestContent = null)
    {
        var requestMessage = new HttpRequestMessage(method, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ETPToken.Token);
        if (!string.IsNullOrWhiteSpace(requestContent))
        {
            requestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
        }

        return requestMessage;
    }
}
