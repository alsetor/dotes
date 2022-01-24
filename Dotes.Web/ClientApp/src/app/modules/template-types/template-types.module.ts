import { NgModule } from '@angular/core';
import { TemplateTypesPageComponent } from './pages/template-types-page/template-types-page.component';
import { SharedModule } from '../../core/shared.module';
import { TypeListComponent } from './components/type-list/type-list.component';
import { TemplateTypesRoutingModule } from './template-types-routing.module';

@NgModule({
  declarations: [TemplateTypesPageComponent, TypeListComponent],
  imports: [SharedModule, TemplateTypesRoutingModule]
})
export class TemplateTypesModule {}
