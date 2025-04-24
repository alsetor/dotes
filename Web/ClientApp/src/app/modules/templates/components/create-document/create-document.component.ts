import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TemplateService } from '../../services/template.service';
import { Template } from '../../../../models/template.model';
import { TagValue } from '../../../../models/tag-value.model';
import { Tag } from '../../../../models/tag.model';
import { TagType, TagTypeDescription } from '../../../../models/tag-type.model';
import { DotesService } from '../../services/dotes.service';

@Component({
  selector: 'create-document-by-template',
  templateUrl: './create-document.component.html',
  styleUrls: ['./create-document.component.css']
})
export class CreateDocumentByTemplateComponent implements OnInit, OnDestroy {
  id: string;
  template: Template = new Template();
  jsonForm: any;

  private ngUnsubscribe = new Subject();

  constructor(
    public templateService: TemplateService,
    public dotesService: DotesService,
    public route: ActivatedRoute,
    public router: Router) { }

  ngOnInit() {
    if (this.route.params != null) {
      this.route.params.pipe(takeUntil(this.ngUnsubscribe)).subscribe((params) => {
        if (params['id'] != null) {
          this.id = params['id'];
          this.getTemplate();
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getTemplate(): void {
    this.templateService.getTemplate(this.id).pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((template) => {
        this.templateService.getTagsFromTemplate(template.id).subscribe(tags => {
          template.tags = tags;
          this.template = template;
        });
      });
  }  

  getTagValueByType(tagType: TagType, value: any): string {
    switch (tagType.toString()) {
      case TagTypeDescription.get(TagType.Image):
        return value.replace(/^data:(.*,)?/, '');
      case TagTypeDescription.get(TagType.Table):
        return JSON.stringify(value);
      default:
        return value as string;
    }
  }

  getType(type: TagType) {
    if (type.toString() == TagTypeDescription.get(TagType.Image))
      return 'file';
    return type.toString().toLowerCase();
  }

  handleFileInput = (files, tag: Tag): boolean => {
    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onloadend = () => {
      tag.value = reader.result;
    };
    return false;
  }

  addRow(tag: Tag) {
    tag.value = tag.value || [];
    let raw: string[] = [];
    tag.cellNames.forEach(cellName => {
      raw.push('');
    });
    tag.value.push(raw);
  }

  deleteRow(tagValue: any[], rawIndex: number) {
    tagValue.splice(rawIndex, 1);
  }

  generateDocument() {
    const tags: TagValue[] = [];

    this.template.tags.forEach(tag => {
      tags.push({
        name: tag.name,
        value: this.getTagValueByType(tag.type, tag.value),
        type: tag.type});
    });
  
    this.dotesService.generateDocument(this.template.id, tags, this.template.name, this.template.fileName);
  }
}
