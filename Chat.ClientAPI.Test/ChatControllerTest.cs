using System.Threading.Tasks;
using Chat.ClientAPI.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Chat.ClientAPI.Test
{
    [TestFixture]
    public class ChatControllerTest
    {
        private ChatApiAsync? _chatApiAsync;

        public ChatControllerTest()
        {
            RestService restService = new RestService();
            _chatApiAsync = new ChatApiAsync(restService);
        }

        [Test]
        public async Task HealthCheck()
        { 
            var response = await _chatApiAsync!.Healthcheck(); 
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        // [Test]
        // public async Task HealthCheck()
        // { 
        //     var response = await _chatApiAsync!.Healthcheck(); 
        //     response.Should().NotBeNull();
        //     response.IsSuccessStatusCode.Should().BeTrue();
        // }
    }
}