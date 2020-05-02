namespace Chat.API.Model
{
    public class MessageToSave
    {
        public string ChannelId { get; set; } // channel
        public string UserId { get; set; }
        public string Message { get; set; }
        public string SocketId { get; set; }
    }
}