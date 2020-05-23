using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chat.ClientAPI.Base;
using Chat.ClientAPI.Entity;

namespace Chat.ClientAPI
{
    public class ChatApiAsync : IChatApiAsync
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

        public async Task<HttpResponseMessage> SendMessage(MessageToSend message)
        {
            return await _restService.MakeRequest(new Request
            {
                Data = message,
                Path = Routes.Chat.SendMessage,
                Method = HttpMethod.Post
            });
        }

        public async Task<List<MessageSent>> GetChatMessages(string channelId)
        {
            return await _restService.MakeRequest<List<MessageSent>>(new Request
            {
                Path = Routes.Chat.BuildMessageGet(channelId),
                Method = HttpMethod.Get
            });
        }
    }
}
