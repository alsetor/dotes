import { Tag } from './tag.model';
import { TemplateType } from './template-type.model';

export class Template {
  constructor() {
      this.type = { id: 0, name: '' };
  }

  id: number;
  uid: any;
  name: string;
  type: TemplateType;
  fileName: string;
  fileBase64: string | ArrayBuffer;
  createdDate: Date;
  updatedDate: Date;
  tags: Tag[];
}
