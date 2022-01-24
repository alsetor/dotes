import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TemplatesComponent } from './components/templates/templates.component';
import { EditTemplateComponent } from './components/edit-template/edit-template.component';
import { CreateDocumentByTemplateComponent } from './components/create-document/create-document.component';
import { TemplatesPageComponent } from './pages/templates-page/templates-page.component';
import { AuthTemplatesGuard } from '../../services/auth/auth-templates.guard';

const routes: Routes = [
  {
    path: '',
    component: TemplatesPageComponent,
    children: [
      { path: '', component: TemplatesComponent, canActivate: [AuthTemplatesGuard] },
      { path: 'edit/:id', component: EditTemplateComponent, canActivate: [AuthTemplatesGuard] },
      { path: 'create', component: EditTemplateComponent, canActivate: [AuthTemplatesGuard] },
      { path: 'createdocument/:id', component: CreateDocumentByTemplateComponent, canActivate: [AuthTemplatesGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TemplateRoutingModule {}
