using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chat.ClientAPI
{
    public interface IChatApiAsync
    {
        Task<HttpResponseMessage> Healthcheck();



        /*
         getUsers(): Observable<User[]> {
            return this.http.get<User[]>(this.baseUrl + '/users' );
          }

        getChatRooms(): Observable<ChatRoom[]> {
            return this.http.get<ChatRoom[]>(this.baseUrl + '/channels/' + environment.pusherKey);
          }

          getChatMessages(chatRoom: ChatRoom): Observable<ChatMessage[]> {
            return this.http.get<ChatMessage[]>(this.baseUrl + '/channel/' + chatRoom.channelId + '/messages');
          }

          sendMessage(channel: ChannelInfo, messageText: string) {
            return this.http.post(this.baseUrl + '/channel/message',
            {
              channelId: channel.chatRoom.channelId,
              userId: this.authService.currentUser.id,
              message: messageText,
              socketId: channel.socketId,
            });
          }

        subscribe(info: ChannelInfo) {
            console.log('Subscribing to channel: '+ info.chatRoom.channelId);
            info.messagesChannel = info.channel.subscribe(info.chatRoom.channelId);
            return info.messagesChannel;
          }

          cleanChannels() {
            this.channels = [];
          }
         */
    }
}
