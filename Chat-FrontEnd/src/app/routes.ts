import { Routes } from '@angular/router';
import { ChatRoomComponent } from './chatRoom/chatRoom.component';

export const appRoutes: Routes = [
    {  path: '', component: ChatRoomComponent },
    {  path: '**', redirectTo: '', pathMatch: 'full' }
];
