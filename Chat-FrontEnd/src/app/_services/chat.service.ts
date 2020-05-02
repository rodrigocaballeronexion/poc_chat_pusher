import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ChatRoom } from '../_models/ChatRoom';
import { ChatMessage } from '../_models/ChatMessage';
import { ChannelInfo } from '../_models/ChannelInfo';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

baseUrl = environment.apiUrl;

public authUrl = this.baseUrl + '/pusher/auth';

constructor(private http: HttpClient,
            private authService: AuthService)
{ }

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
}
