import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  users: User[];

  constructor(public authService: AuthService) { }

  ngOnInit() {
    this.authService.getUsers()
    .subscribe((res: User[]) => {
      this.users = res;
    }, error => {
      console.log(error.error);
    });
  }

  isLoggedIn() {
    return this.authService.loggedIn();
  }

  login(user: User){
    this.authService.logIn(user);
  }

}
