using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.API.Data
{
    public class Subscriber
    {
        public string SubscriberId { get; set; }

        public string ChatName { get; set; }

        public List<ChannelSubscriber> ChannelSubscribers { get; set; }
    }
}
