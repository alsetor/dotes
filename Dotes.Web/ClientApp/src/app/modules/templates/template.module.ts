import { NgModule } from '@angular/core';
import { TemplateTagComponent } from './components/template-tag/template-tag.component';
import { CreateDocumentByTemplateComponent } from './components/create-document/create-document.component';
import { EditTemplateComponent } from './components/edit-template/edit-template.component';
import { TemplatesComponent } from './components/templates/templates.component';
import { TemplateRoutingModule } from './template-routing.module';
import { TemplatesPageComponent } from './pages/templates-page/templates-page.component';
import { SharedModule } from '../../core/shared.module';

@NgModule({
  declarations: [
    TemplatesComponent,
    EditTemplateComponent,
    TemplateTagComponent,
    CreateDocumentByTemplateComponent,
    TemplatesPageComponent
  ],
  imports: [TemplateRoutingModule, SharedModule]
})
export class TemplateModule {}
