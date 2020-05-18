using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chat.ClientAPI.Base;

namespace Chat.ClientAPI
{
    public class ChatApiAsync : RestService, IChatApiAsync
    {
        private readonly IRestService _restService;

        public ChatApiAsync(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<HttpResponseMessage> Healthcheck()
        {
            return await _restService.MakeRequest(new Request
            {
                Path = Routes.Chat.HealthCheck,
                Method = HttpMethod.Get
            });
        }
    }
}
