import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';
import { BaseService } from '../base.service';
import { ConfigService } from '../config.service';
import { Observable, of } from 'rxjs';
import { NzNotificationService } from 'ng-zorro-antd';

export const TOKEN_NAME: string = 'auth_template_token';

@Injectable()
export class AuthTemplatesService extends BaseService {
  baseUrl: string = '';
  private loggedIn = false;

  constructor(private http: HttpClient, private configService: ConfigService, private notification: NzNotificationService) {
    super();
    this.baseUrl = configService.getApiURI();

    const hasAuthCookies =
      (document.cookie.split(';') || []).filter((c) => {
        let cookie = c.trim().split('=');
        return (cookie[0] === 'login' || cookie[0] === 'password') && !!cookie[1];
      }).length === 2;

    this.loggedIn = hasAuthCookies && !!localStorage.getItem(TOKEN_NAME) && !this.isTokenExpired();
  }

  login(login, password) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    const credentials = { login, password };
    return this.http
      .post(this.baseUrl + '/auth/login', credentials, { headers })
      .map((res) => {
        localStorage.setItem(TOKEN_NAME, res['auth_token']);
        this.loggedIn = true;
        return true;
      });
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
    this.loggedIn = false;
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  getTokenExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const date = this.getTokenExpirationDate(token);
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  getToken(): string {
    return localStorage.getItem(TOKEN_NAME);
  }
}
