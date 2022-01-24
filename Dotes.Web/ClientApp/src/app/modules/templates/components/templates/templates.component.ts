import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { TemplateService } from '../../services/template.service';
import { Template } from '../../../../models/template.model';
import { TemplateType } from '../../../../models/template-type.model';
import { TemplateTypesService } from '../../../template-types/services/template-types.service';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css']
})
export class TemplatesComponent implements OnInit, OnDestroy {
  templateTypes: TemplateType[];
  documentTemplates: Template[] = null;
  showPagination: boolean = false;
  loading: boolean = true;
  templateType: string = '';

  private ngUnsubscribe = new Subject();

  constructor(
    private templateService: TemplateService,
    private router: Router,
    private route: ActivatedRoute,
    private templateTypesService: TemplateTypesService
  ) {}

  ngOnInit() {
    this.getTemplates();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getTemplates(): void {
    this.loading = true;
    this.templateService
      .getTemplates()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((documentTemplates) => {
        this.documentTemplates = documentTemplates;
        this.showPagination = documentTemplates.length > 10;
        this.sort({ key: 'createdDate', value: 'descend' });
        this.loading = false;
      });

    this.getTemplateTypes();
  }

  deleteTemplate(id: number, index: number): void {
    this.templateService
      .deleteTemplate(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.documentTemplates.splice(index, 1);
        this.documentTemplates = [...this.documentTemplates];
      });
  }

  sort(sort: { key: string; value: string }): void {
    if (sort.value) {
      this.documentTemplates.sort((a, b) => {
        switch (sort.key) {
          case 'name':
            if (a.name.toLowerCase() > b.name.toLowerCase()) {
              return sort.value === 'descend' ? -1 : 1;
            }
            if (a.name.toLowerCase() < b.name.toLowerCase()) {
              return sort.value === 'descend' ? 1 : -1;
            }
            return 0;
          case 'createdDate':
            if (a.createdDate > b.createdDate) {
              return sort.value === 'descend' ? -1 : 1;
            }
            if (a.createdDate < b.createdDate) {
              return sort.value === 'descend' ? 1 : -1;
            }
            return 0;
          case 'type':
            if (a.type.id > b.type.id) {
              return sort.value === 'descend' ? -1 : 1;
            }
            if (a.type.id < b.type.id) {
              return sort.value === 'descend' ? 1 : -1;
            }
            return 0;
          default:
            if (new Date(a.updatedDate) > new Date(b.updatedDate)) {
              return sort.value === 'descend' ? -1 : 1;
            }
            if (new Date(a.updatedDate) < new Date(b.updatedDate)) {
              return sort.value === 'descend' ? 1 : -1;
            }
            return 0;
        }
      });

      this.documentTemplates = [...this.documentTemplates];
    }
  }

  downloadTemplate(templateId: number, fileName: string) {
    this.templateService.getFileByTemplateId(templateId, fileName);
  }

  templateTypeChange(type: number) {
    this.router
      .navigate([], {
        relativeTo: this.route,
        queryParams: {
          typeId: type
        },
        queryParamsHandling: 'merge'
      })
      .then(() => this.getTemplates());
  }

  getTemplateTypes() {
    this.templateTypesService
      .getTemplateTypes()
      .takeUntil(this.ngUnsubscribe)
      .subscribe((result) => {
        this.templateTypes = result;
        this.loading = false;
      });
  }
}
