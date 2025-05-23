import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadFile, NzNotificationService } from 'ng-zorro-antd';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TemplateService } from '../../services/template.service';
import { Template } from '../../../../models/template.model';
import { Tag } from '../../../../models/tag.model';
import { TagType } from '../../../../models/tag-type.model';

@Component({
  selector: 'app-edit-template',
  templateUrl: './edit-template.component.html',
  styleUrls: ['./edit-template.component.css']
})
export class EditTemplateComponent implements OnInit, OnDestroy {
  id: string;
  fileList: UploadFile[] = [];
  isUpdateMode: boolean = false;
  //showReminder: boolean = true;
  uploaded: boolean = false;
  template: Template = new Template();
  initialTags: Tag[];
  name: string;
  fileBase64: string | ArrayBuffer;

  private ngUnsubscribe = new Subject();

  constructor(
    private templateService: TemplateService,
    private route: ActivatedRoute,
    private router: Router,
    private message: NzNotificationService,
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    if (this.route.params != null) {
      this.route.params.pipe(takeUntil(this.ngUnsubscribe)).subscribe((params) => {
        if (params['id'] != null) {
          this.id = params['id'];
          this.isUpdateMode = true;
          this.getTemplate();
        } else {
          this.template.tags = [];
          this.addTemplateTag();
        }
      });
    }
  }

  getTemplate(): void {
    this.templateService.getTemplate(this.id).pipe(takeUntil(this.ngUnsubscribe)).subscribe((template) => {
      this.template = template;
      this.initialTags = template.tags;
      this.name = template.name;
      this.fileBase64 = template.fileBase64;
    });
  }

  addTemplateTag() {
    let templateTag: Tag = { name: '', type: TagType.String, label: '', cellNames: [], value: null };
    this.template.tags.push(templateTag);
  }

  deleteTemplateTag(index: number): void {
    this.template.tags.splice(index, 1);
  }

  createTemplate() {
    this.templateService.createTemplate(this.template).pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => {
      this.router.navigate(['/templates']);
    });
  }

  updateTemplate() {
    this.templateService.updateTemplate(this.template).pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => {
      this.router.navigate(['/templates']);
      this.message.create('info', 'Template updated', '');
    });
  }

  isAddButtonDisabled(): boolean {
    return (
      this.template.tags &&
      this.template.tags.some(
        (t) => t.name.length < 1 || (t.type == TagType.Table && t.cellNames.some((n) => n.length < 1))
      )
    );
  }

  isDeleteButtonDisabled(): boolean {
    return this.template.tags && this.template.tags.length < 2;
  }

  isChooseFileButtonDisabled(): boolean {
    return this.fileList.length > 0;
  }

  isSaveButtonDisabled(): boolean {
    return (
      (!this.isChooseFileButtonDisabled() && !this.isUpdateMode) ||
      this.isAddButtonDisabled() ||
      !this.template.name
    );
  }

  isUpdateButtonDisabled(): boolean {
    return (
      ((!this.isChooseFileButtonDisabled() && !this.isUpdateMode) ||
      this.isAddButtonDisabled() ||
      !this.template.name ||
      !this.ifHasChangesInTags()) &&
      !this.ifHasChangesInName() &&
      !this.ifHasChangesInFile()
    );
  }

  ifHasChangesInTags(): boolean {
    return JSON.stringify(this.initialTags) !== JSON.stringify(this.template.tags);
  }

  ifHasChangesInName(): boolean {
    return this.name !== this.template.name;
  }

  ifHasChangesInFile(): boolean {
    return this.fileBase64 !== this.template.fileBase64;
  }

  beforeUpload = (file: UploadFile): boolean => {
    this.uploaded = false;
    this.fileList = this.fileList.concat(file);
    this.template.fileName = file.name;
    this.handleUpload(file);
    return false;
  };

  handleUpload(file): void {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = () => {
      this.template.fileBase64 = reader.result;
      this.uploaded = true;
    };
  }

  downloadTemplate() {
    this.templateService.downloadTemplate(this.template.id, this.template.fileName);
  }
}
