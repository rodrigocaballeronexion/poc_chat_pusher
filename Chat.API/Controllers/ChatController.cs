using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.API.Data;
using Chat.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PusherServer;

namespace Chat.API.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly ChatDbContext _dbContext;

        public ChatController(ILogger<ChatController> logger
            , ChatDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("hello")]
        public IActionResult Healthcheck()
        {
            return Ok();
        }

        [HttpPost]
        [Route("pusher/auth")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> AuthForChannel([FromForm] AuthRequest playerPackage)
        {
            var channel = await _dbContext.Channels
                                .Include(p => p.ChannelApp)
                                .FirstOrDefaultAsync(p => p.ChannelId == playerPackage.channel_name);

            if(channel == null)
            {
                return BadRequest("Unknown channel" + playerPackage.channel_name);
            }

            var pusher = GetPusher(channel);
            var auth = pusher.Authenticate(playerPackage.channel_name, playerPackage.socket_id);

            return Ok(auth);
        }

        [HttpGet]
        [Route("channels/{key}")]
        public async Task<IActionResult> GetChannels(string key)
        {
            var channelApp = await _dbContext.ChannelApps.FirstOrDefaultAsync(p => p.Key == key);
            if(channelApp == null)
                return BadRequest("Invalid channel key");

            var channels = await _dbContext.Channels.ToListAsync();
            return Ok(channels.Select(it => new {
                ChannelId = it.ChannelId,
                Name = it.Name
            }));
        }

        // [HttpPost]
        // [Route("channel/create")]
        // public async Task<IActionResult> CreateChannel(CreateChannelDTO dto)
        // {

        // }

        [HttpPost]
        [Route("channel/join")]
        public async Task<IActionResult> JoinChannel(string who, string id)
        {
            if(string.IsNullOrEmpty(who))
                return BadRequest("Invalid user");
            if(string.IsNullOrEmpty(id))
                return BadRequest("Invalid channel");

            var channel = await _dbContext.Channels.FirstOrDefaultAsync(p => p.ChannelId == id);
            if(channel == null)
                return BadRequest("Inexistent channel");

            var subscriber = await _dbContext.Subscribers.Include(p => p.ChannelSubscribers)
                .FirstOrDefaultAsync(p => p.ChatName.ToLower() == who.ToLower().Trim());

            if(subscriber == null)
            {
                subscriber = new Subscriber
                {
                    SubscriberId = Guid.NewGuid().ToString(),
                    ChatName = who.Trim(),
                    ChannelSubscribers = new List<ChannelSubscriber>()
                };

                subscriber.ChannelSubscribers.Add(new ChannelSubscriber{
                    Channel = channel,
                    Subscriber = subscriber
                });

                await _dbContext.Subscribers.AddAsync(subscriber);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if(!subscriber.ChannelSubscribers.Any(p => p.ChannelId == channel.ChannelId))
                {
                    subscriber.ChannelSubscribers.Add(
                        new ChannelSubscriber{
                        Channel = channel,
                        Subscriber = subscriber
                    });
                    
                    _dbContext.Subscribers.Update(subscriber);
                    await _dbContext.SaveChangesAsync();
                }
            }
            
            return Ok(subscriber.SubscriberId);
        }

        [HttpGet]
        [Route("channel/{id}/messages")]
        public async Task<IActionResult> GetMessages(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest("Invalid channel");

            var messages = await _dbContext.Messages.Include(p => p.Sender)
                            .Where(p => p.ChannelId == id).ToListAsync();

            return Ok(
                messages.Select( it => CreateMessage(it))
            );
        }

        [HttpPost]
        [Route("channel/message")]
        public async Task<IActionResult> SaveMessage(MessageToSave message)
        {
            if(string.IsNullOrEmpty(message.ChannelId))
                return BadRequest("Invalid channel");

            if(string.IsNullOrEmpty(message.Message))            
                return BadRequest("Invalid message");

            if(string.IsNullOrEmpty(message.UserId))
                return BadRequest("Invalid User");

            var channel = await _dbContext.Channels
                                .Include(p => p.ChannelApp)
                                .FirstOrDefaultAsync(p => p.ChannelId == message.ChannelId);
            if(channel == null)
                return BadRequest("Inexistent channel");

            var subscriber = await _dbContext.Subscribers.FirstOrDefaultAsync(p => p.SubscriberId == message.UserId);
            if(subscriber == null)
            {
                subscriber = new Subscriber{
                    SubscriberId = message.UserId,
                    ChatName = message.UserId
                };

                await _dbContext.Subscribers.AddAsync(subscriber);
            }

            var messageToSave = new Message
            {
                Content = message.Message,
                Channel = channel,
                Sender = subscriber,
                When = DateTime.Now
            };

            await _dbContext.Messages.AddAsync(messageToSave);
            await _dbContext.SaveChangesAsync();

            var pusher = GetPusher(channel);
         
            // to avoid self reference on json convert
            var responseMessage = CreateMessage(messageToSave);
            
            await pusher.TriggerAsync(
                channel.ChannelId,
                "new_message",
                responseMessage);

            return Ok(responseMessage);
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            var subscribers = await _dbContext.Subscribers.ToListAsync();
            return Ok(subscribers.Select(p => new {
                Id = p.SubscriberId,
                Name = p.ChatName
            }));
        }
        
        private object CreateMessage(Message it)
        {
            return new {
                    Id = it.Id,
                    Message = it.Content,
                    UserId = it.SenderId,
                    Name = it.Sender.ChatName,
                    When = it.When
                };
        }

        private Pusher GetPusher(Channel channel)
        {
            var secret = GetSecret();
            return new Pusher(
                channel.ChannelApp.AppId,
                channel.ChannelApp.Key,
                secret,
                new PusherOptions{
                    Cluster = channel.ChannelApp.Cluster
                }
            );
        }

        private string GetSecret() 
        => Environment.GetEnvironmentVariable("PUSHER_SECRET");
    }
}
