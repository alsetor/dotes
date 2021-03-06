import { Component } from '@angular/core';
import { AuthService } from './services/auth/auth.service';
import { SideMenuService } from './services/side-menu.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  year = new Date().getFullYear();

  constructor(private authService: AuthService, private sideMenuService: SideMenuService) {}

  get isCollapsed() {
    return this.sideMenuService.isCollapsed;
  }

  set isCollapsed(value: boolean) {
    this.sideMenuService.isCollapsed = value;
  }

  isLoggedIn() {
    return this.authService.isLoggedIn;
  }

  logout() {
    this.authService.logout();
  }

  get deployUrl() {
    return environment.deployUrl;
  }
}
