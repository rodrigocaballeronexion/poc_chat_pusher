using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chat.ClientAPI.Base
{
    public class RestService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<TResponseType> MakeRequest<TResponseType>(Request request) where TResponseType : class
        {
            var responseObject = default(TResponseType);

            var responseMessage = await MakeHttpRequest(request);

            if (!responseMessage.IsSuccessStatusCode) return null;

            var response = await responseMessage.Content.ReadAsStringAsync();
            responseObject = JsonConvert.DeserializeObject<TResponseType>(response);

            return responseObject;
        }

        private async Task<HttpResponseMessage> MakeHttpRequest(Request request)
        {
            var httpMessage = new HttpRequestMessage(request.Method, request.Path);

            if (request.Data != null)
            {
                httpMessage.Content = new StringContent(request.Data.ToString(), Encoding.UTF8, "application/json");
            }

            var responseMessage =
                await _httpClient.SendAsync(httpMessage);
            
            return responseMessage;
        }
    }
}
