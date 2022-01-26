import { Component, OnInit, Input } from '@angular/core';
import { TagType, TagTypeDescription } from '../../../../models/tag-type.model';
import { Tag } from '../../../../models/tag.model';

@Component({
  selector: 'app-template-tag',
  templateUrl: './template-tag.component.html',
  styleUrls: ['./template-tag.component.css']
})
export class TemplateTagComponent implements OnInit {
  @Input() templateTag: Tag;
  tagTypes = [
    { id: TagTypeDescription.get(TagType.String), name: TagTypeDescription.get(TagType.String) },
    { id: TagTypeDescription.get(TagType.Table), name: TagTypeDescription.get(TagType.Table) },
    { id: TagTypeDescription.get(TagType.Image), name: TagTypeDescription.get(TagType.Image) }
  ];

  constructor() {}

  ngOnInit() {}

  addTableCell(): void {
    this.templateTag.cellNames.push('');
  }

  deleteTableCell(index: number): void {
    this.templateTag.cellNames.splice(index, 1);
  }

  isAddButtonDisabled(): boolean {
    return this.templateTag.cellNames && this.templateTag.cellNames.some((n) => n.length < 1);
  }

  isDeleteButtonDisabled(): boolean {
    return this.templateTag.cellNames && this.templateTag.cellNames.length < 2;
  }

  trackByIndex(index: number, obj: any): any {
    return index;
  }

  templateTagTypeChanged(): void {
    if (this.templateTag.type == TagType.Table) {
      this.addTableCell();
    } else {
      this.templateTag.cellNames = [];
    }
  }
}
