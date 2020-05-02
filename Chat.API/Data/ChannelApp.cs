using System.Collections.Generic;

namespace Chat.API.Data
{
    public class ChannelApp
    {
        public string AppId { get; set; }
        
        public string Key { get; set; }

        public string Secret { get; set; }

        public string Cluster { get; set; }

        public string Name { get; set; }

        public List<Channel> Channels { get; set; }
    }
}