import { ChatMessage } from './ChatMessage';
import { ChatRoom } from './ChatRoom';

export class ChannelInfo {
    key: string;
    chatRoom: ChatRoom;
    channel: any;
    socketId: string;
    messagesChannel: any;
    messagesReceived: ChatMessage[];
}
