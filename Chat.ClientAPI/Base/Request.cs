using System.Collections.Generic;
using System.Net.Http;

namespace Chat.ClientAPI.Base
{
    public class Request
    {
        public string Path { get; set; }
        
        public HttpMethod Method { get; set; }
        
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        
        public object Data { get; set; }
    }
}