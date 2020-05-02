using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.API.Data
{
    public class Channel
    {
        public string ChannelId { get; set; }

        public string Name { get; set; }

        public string ChannelAppId { get; set; }

        public ChannelApp ChannelApp { get; set; }

        public List<ChannelSubscriber> ChannelSubscribers { get; set; }

        public List<Message> Messages { get; set; }
    }
}
