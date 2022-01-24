import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N, ru_RU, NZ_ICONS } from 'ng-zorro-antd';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { registerLocaleData } from '@angular/common';
import ru from '@angular/common/locales/ru';
import { ConfigService } from './services/config.service';

import { NzConfig, NZ_CONFIG } from 'ng-zorro-antd';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './core/shared.module';
import { CacheInterceptor } from './core/cacheinterceptor';
import { AuthTemplatesGuard } from './services/auth/auth-templates.guard';
import { AuthTemplatesService } from './services/auth/auth-templates.service';

const ngZorroConfig: NzConfig = {
  notification: { nzDuration: 15000 }
};

if (ru && ru[5] && ru[5][1]) {
  ru[5][1] = ['янв.', 'февр.', 'мар.', 'апр.', 'май', 'июн.', 'июл.', 'авг.', 'сент.', 'окт.', 'нояб.', 'дек.'];
}
registerLocaleData(ru);

const antDesignIcons = AllIcons as {
  [key: string]: IconDefinition;
};
const icons: IconDefinition[] = Object.keys(antDesignIcons).map((key) => antDesignIcons[key]);

@NgModule({
  declarations: [AppComponent],
  imports: [AppRoutingModule, SharedModule, HttpClientModule, BrowserModule, BrowserAnimationsModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true },
    { provide: NZ_I18N, useValue: ru_RU },
    { provide: NZ_CONFIG, useValue: ngZorroConfig },
    { provide: NZ_ICONS, useValue: icons },
    ConfigService,
    AuthTemplatesGuard,
    AuthTemplatesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
