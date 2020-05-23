namespace Chat.ClientAPI.Entity
{
    public class MessageToSend
    {
        public string ChannelId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string SocketId { get; set; }
    }
}