import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl;
  currentUser: User;

constructor(private http: HttpClient) { }

  loggedIn() {
    const user = localStorage.getItem('user');
    return user !== null && this.currentUser != null;
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/users' );
  }

  logIn(user: User) {
    this.currentUser = user;
    localStorage.setItem('user', JSON.stringify(user));
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser = null;
  }
}
