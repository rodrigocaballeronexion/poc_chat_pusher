import { AuthService } from './../_services/auth.service';
import { ChatMessage } from './../_models/ChatMessage';
import { ChatRoom } from './../_models/ChatRoom';
import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatService } from '../_services/chat.service';
import { PusherService } from '../_services/pusher.service';
import { ChannelInfo } from '../_models/ChannelInfo';

@Component({
  selector: 'app-chat-room',
  templateUrl: './chatRoom.component.html',
  styleUrls: ['./chatRoom.component.css']
})
export class ChatRoomComponent implements OnInit, AfterViewChecked {
  @ViewChild('msg_history') private chatMessagesBox: ElementRef;

  channels: Array<ChannelInfo> = [];

  currentChannel: ChannelInfo;
  messageText: string;

  constructor(public chatService: ChatService
            , public pusherService: PusherService
            , public authService: AuthService) {
  }

  ngOnInit() {

    this.chatService.getChatRooms()
    .subscribe((res: ChatRoom[]) => {
       this.channels = this.pusherService.initChannels(res);
    }, error => {
      console.log(error.error);
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  checkChannel(channel: ChannelInfo) {
    this.pusherService.subscribe(channel);

    this.chatService.getChatMessages(channel.chatRoom)
      .subscribe((res: ChatMessage[]) => {
        channel.messagesReceived = res;
      }, error => {
        console.log(error.error);
      });

    this.currentChannel = channel;
    this.scrollToBottom();

    this.bind_client_events(this.currentChannel);
  }

  bind_client_events(info: ChannelInfo){

    info.channel.bind('new_message', (msg) =>  {
      if (this.currentChannel && msg)
      {
          const model = new ChatMessage();
          model.id = msg.Id;
          model.message = msg.Message;
          model.userId = msg.UserId;
          model.name = msg.Name;
          model.when = msg.When;
          this.currentChannel.messagesReceived.push(model);

          this.scrollToBottom();
      }
    });

    info.channel.bind('message_delivered', (msg) => {
        console.log(msg);
    });
  }

  sendMessage(messageText: string)
  {
    this.chatService.sendMessage(this.currentChannel, messageText)
    .subscribe((res: any) => {
      console.log('message sent');
      this.messageText = '';
      this.scrollToBottom();
    }, error => {
      console.log(error.error);
    });
  }

  logout()
  {
    this.authService.logout();
    this.pusherService.cleanChannels();
    this.channels = [];
  }

  isFromCurrentUser(message: ChatMessage) {
    return message.userId === this.authService.currentUser.id;
  }

  scrollToBottom() {
    if (this.chatMessagesBox) {
      this.chatMessagesBox.nativeElement.scrollTop = this.chatMessagesBox.nativeElement.scrollHeight;
    }
  }
}
