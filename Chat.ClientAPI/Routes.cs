namespace Chat.ClientAPI
{
    public class Routes
    {
        public static class Chat
        {
            public const string HealthCheck = "/hello"; // GET

            public const string SendMessage = "/channel/message"; // POST

            public const string Messages = "/channel/{channelId}/messages/"; // POST

            public static string BuildMessageGet(string channelId)
            {
                return Messages.Replace("{channelId}", channelId);
            }
        }
    }
}