using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chat.ClientAPI.Entity;

namespace Chat.ClientAPI
{
    public interface IChatApiAsync
    {
        Task<HttpResponseMessage> Healthcheck();

        Task<HttpResponseMessage> SendMessage(MessageToSend message);

        Task<List<MessageSent>> GetChatMessages(string channelId);

    }
}
