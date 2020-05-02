using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ChannelApp> ChannelApps { get; set; }

        public virtual DbSet<Channel> Channels { get; set; }

        public virtual DbSet<Subscriber> Subscribers { get; set; }

        public virtual DbSet<ChannelSubscriber> ChannelSubscribers { get; set; }
  
        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ChannelApp
            var channelAppBuilder = modelBuilder.Entity<ChannelApp>();
            channelAppBuilder.HasKey(p => p.AppId);
            channelAppBuilder.Property(p => p.Key).IsRequired();
            channelAppBuilder.Property(p => p.Name).IsRequired().HasMaxLength(164);
            channelAppBuilder.Property(p => p.Secret).IsRequired();
            channelAppBuilder.Property(p => p.Cluster).IsRequired();
            channelAppBuilder.HasMany(p => p.Channels).WithOne(p => p.ChannelApp).HasForeignKey(p => p.ChannelAppId);

            // Channel
            var channelBuilder = modelBuilder.Entity<Channel>();
            channelBuilder.HasKey(p => p.ChannelId);
            channelBuilder.Property(p => p.Name).IsRequired().HasMaxLength(164);
            channelBuilder.HasMany(p => p.ChannelSubscribers).WithOne(p => p.Channel).HasForeignKey(p => p.ChannelId);
            
            // Subscriber
            var subscriberBuilder = modelBuilder.Entity<Subscriber>();
            subscriberBuilder.Property(p => p.ChatName).IsRequired();
            subscriberBuilder.HasMany(p => p.ChannelSubscribers).WithOne(p => p.Subscriber).HasForeignKey(p => p.SubscriberId);
            
            // ChannelSubscriber
            var channelSubscriberBuilder = modelBuilder.Entity<ChannelSubscriber>();
            channelSubscriberBuilder.HasKey(cs => new { cs.ChannelId, cs.SubscriberId });
            channelSubscriberBuilder.HasOne(p => p.Channel)
                                    .WithMany(p => p.ChannelSubscribers)
                                    .HasForeignKey(p => p.ChannelId);
            channelSubscriberBuilder.HasOne(p => p.Subscriber)
                             .WithMany(p => p.ChannelSubscribers)
                             .HasForeignKey(p => p.SubscriberId);

            // Messages
            var messagesBuilder = modelBuilder.Entity<Message>();

            messagesBuilder.HasOne(p => p.Sender);
            messagesBuilder.HasOne(p => p.Channel).WithMany(p => p.Messages).HasForeignKey(p => p.ChannelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
