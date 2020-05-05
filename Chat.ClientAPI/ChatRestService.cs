using System;
using System.Collections.Generic;
using System.Text;
using Chat.ClientAPI.Base;

namespace Chat.ClientAPI
{
    public class ChatRestService : RestService, IChatRestService
    {
        private readonly IRestService _restService;

        public ChatRestService(IRestService restService)
        {
            _restService = restService;
        }
    }
}
