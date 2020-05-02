namespace Chat.API.Data
{
    public class ChannelSubscriber
    {
        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        public string SubscriberId { get; set; }

        public Subscriber Subscriber { get; set; }
    }
}