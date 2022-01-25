import { TemplateTypesPageComponent } from './pages/template-types-page/template-types-page.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from '../../services/auth/auth.guard';
import { TypeListComponent } from './components/type-list/type-list.component';

const routes: Routes = [
  {
    path: '',
    component: TemplateTypesPageComponent,
    children: [{ path: '', component: TypeListComponent, canActivate: [AuthGuard] }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TemplateTypesRoutingModule {}
