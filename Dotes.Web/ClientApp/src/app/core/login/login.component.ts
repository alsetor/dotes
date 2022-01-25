import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { first, takeUntil } from 'rxjs/operators';
import { AuthService } from '../../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginTemplateComponent implements OnInit, OnDestroy {
  login: string;
  password: string;
  returnUrl: string;
  passwordVisible = false;

  private ngUnsubscribe = new Subject();

  errors: HttpErrorResponse | string;

  ngOnInit() {
    this.route.queryParams.pipe(takeUntil(this.ngUnsubscribe)).subscribe((params) => {
      this.returnUrl = params['returnUrl'] || '/';
      if (this.authService.isLoggedIn) {
        this.router.navigateByUrl(this.returnUrl);
      }
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {}

  auth() {
    if (this.login && this.password) {
      this.authService
        .login(this.login, this.password)
        .pipe(first())
        .subscribe(
          (result) => {
            if (result) {
              this.router.navigateByUrl(this.returnUrl);
            }
          },
          (error) => (this.errors = error)
        );
    }
  }

  get submitBtnDisabled() {
    return !(this.login && this.password);
  }
}
