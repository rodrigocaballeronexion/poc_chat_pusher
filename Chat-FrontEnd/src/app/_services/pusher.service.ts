import { Injectable } from '@angular/core';
import { ChannelInfo } from '../_models/ChannelInfo';
import { ChatService } from './chat.service';
import { ChatRoom } from '../_models/ChatRoom';
import { environment } from '../../environments/environment';
declare const Pusher: any;

@Injectable({
  providedIn: 'root'
})
export class PusherService {

  channels: Array<ChannelInfo> = [];

constructor(private chatService: ChatService) { }

initChannels(chatRooms: ChatRoom[]): Array<ChannelInfo> {

    if (chatRooms)
    {
      console.log('initiating chat rooms');
      chatRooms.forEach(item => {
        const channelInfo = new ChannelInfo();
        channelInfo.key = item.channelId;
        channelInfo.chatRoom = item;
        channelInfo.messagesReceived = [];
        // pusher
        channelInfo.channel = new Pusher(environment.pusherKey,
        {
          authEndpoint: this.chatService.authUrl,
          forceTLS: false,
        });

        channelInfo.channel.connection.bind('connected', () => {
          channelInfo.socketId = channelInfo.channel.connection.socket_id;
          console.log('socket:' + channelInfo.channel.connection.socket_id);
        });

        this.channels.push(channelInfo);
      });

      return this.channels;
    }
  }

  subscribe(info: ChannelInfo) {
    console.log('Subscribing to channel: ' + info.chatRoom.channelId);
    info.messagesChannel = info.channel.subscribe(info.chatRoom.channelId);
    return info.messagesChannel;
  }

  cleanChannels() {
    this.channels = [];
  }

}
