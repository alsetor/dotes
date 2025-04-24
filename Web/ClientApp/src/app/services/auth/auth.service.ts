import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

export const TOKEN_NAME: string = 'auth_token';
export const LOGGED_IN_NAME: string = 'logged_in';

@Injectable()
export class AuthService {
  private loggedInSubject: BehaviorSubject<boolean>;

  constructor(private http: HttpClient, private router: Router) {
    this.loggedInSubject = new BehaviorSubject<boolean>(JSON.parse(localStorage.getItem(LOGGED_IN_NAME)));
  }

  login(login, password) {
    return this.http.post<any>(`/auth/login`, { login, password })
      .pipe(map(result => {
        localStorage.setItem(TOKEN_NAME, window.btoa(login + ':' + password));
        localStorage.setItem(LOGGED_IN_NAME, JSON.stringify(true));
        this.loggedInSubject.next(true);
        return true;
      }));
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
    this.loggedInSubject.next(null);
    this.router.navigate(['/login']);
  }

  public get isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }  

  getToken(): string {
    return localStorage.getItem(TOKEN_NAME);
  }
}
