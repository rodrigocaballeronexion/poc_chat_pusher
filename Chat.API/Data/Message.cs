using System;

namespace Chat.API.Data
{
    public class Message
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        public string SenderId { get; set; }

        public Subscriber Sender { get; set; }

        public DateTime When { get; set; }
    }
}