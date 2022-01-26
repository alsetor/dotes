import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TemplateTypesService } from '../../services/template-types.service';
import { TemplateType } from '../../../../models/template-type.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NzModalService, NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'app-type-list',
  templateUrl: './type-list.component.html',
  styleUrls: ['./type-list.component.css']
})
export class TypeListComponent implements OnInit, OnDestroy {
  templateTypes: TemplateType[];
  newTemplateTypeName: string;
  showPagination: boolean = false;
  loading: boolean = true;

  private ngUnsubscribe = new Subject();

  @ViewChild('createModal', { static: false }) createModal;
  @ViewChild('updateModal', { static: false }) updateModal;

  constructor(
    private templateTypesService: TemplateTypesService,
    private modal: NzModalService,
    private message: NzNotificationService
  ) {}

  ngOnInit() {
    this.getTemplateTypes();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getTemplateTypes() {
    this.templateTypesService.getTemplateTypes().pipe(takeUntil(this.ngUnsubscribe)).subscribe((result) => {
      this.templateTypes = result;
      this.loading = false;
    });
  }

  create() {
    this.newTemplateTypeName = null;
    this.modal.confirm({
      nzTitle: 'Input type name',
      nzContent: this.createModal,
      nzOnOk: () =>
        this.templateTypesService.createTemplateType(this.newTemplateTypeName).subscribe((result) => {
          if (result) {
            this.loading = true;
            this.getTemplateTypes();
            this.message.success('Type name created', '');
          }
        })
    });
  }

  update(type: TemplateType) {
    this.newTemplateTypeName = type.name;
    this.modal.confirm({
      nzTitle: 'Change type name',
      nzContent: this.updateModal,
      nzOnOk: () => {
        type.name = this.newTemplateTypeName;
        this.templateTypesService.updateTemplateType(type).subscribe((_) => {
          this.newTemplateTypeName = null;
          this.loading = true;
          this.getTemplateTypes();
          this.message.success('Type name changed', '');
        });
      }
    });
  }

  delete(type: TemplateType) {
    this.templateTypesService.deleteTemplateType(type.id).subscribe((result) => {
      if (result) {
        this.getTemplateTypes();
        this.message.success('Type name deleted', '');
      }
    });
  }
}
