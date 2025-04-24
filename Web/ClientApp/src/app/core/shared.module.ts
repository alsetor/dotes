import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgZorroAntdModule, NZ_I18N, ru_RU } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzResizableModule } from 'ng-zorro-antd/resizable';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginTemplateComponent } from './login/login.component';

@NgModule({
  declarations: [NotFoundComponent, LoginTemplateComponent],
  providers: [{ provide: NZ_I18N, useValue: ru_RU }],
  exports: [
    CommonModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    FormsModule,
    NzResizableModule,
    NotFoundComponent,
    LoginTemplateComponent,
  ],
  imports: [
    CommonModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    FormsModule,
    NzResizableModule,
    RouterModule
  ]
})
export class SharedModule {}
