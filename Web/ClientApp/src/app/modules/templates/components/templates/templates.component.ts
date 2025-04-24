import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { TemplateService } from '../../services/template.service';
import { Template } from '../../../../models/template.model';

@Component({
  selector: 'app-templates',
  templateUrl: './templates.component.html',
  styleUrls: ['./templates.component.css']
})
export class TemplatesComponent implements OnInit, OnDestroy {
  documentTemplates: Template[] = null;
  showPagination: boolean = false;
  loading: boolean = true;

  private ngUnsubscribe = new Subject();

  constructor(
    private templateService: TemplateService,
    private router: Router,
    private route: ActivatedRoute,
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
    this.templateService.getTemplates().pipe(takeUntil(this.ngUnsubscribe)).subscribe((templates) => {
      this.documentTemplates = templates;
      this.showPagination = templates.length > 10;
      this.sort({ key: 'createdDate', value: 'descend' });
      this.loading = false;
    });
  }

  deleteTemplate(id: string, index: number): void {
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

  downloadTemplate(templateId: string, fileName: string) {
    this.templateService.downloadTemplate(templateId, fileName);
  }
}
