import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { LoginTemplateComponent } from './core/login/login.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'templates',
    pathMatch: 'full'
  },
  {
    path: 'templates',
    loadChildren: () => import('./modules/templates/template.module').then((m) => m.TemplateModule)
  },
  {
    path: 'types',
    loadChildren: () => import('./modules/template-types/template-types.module').then((m) => m.TemplateTypesModule)
  },
  { path: 'login', component: LoginTemplateComponent },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' },
  { path: 'not-found', component: NotFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      enableTracing: false,
      paramsInheritanceStrategy: 'always',
      preloadingStrategy: PreloadAllModules,
      scrollPositionRestoration: 'enabled'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
