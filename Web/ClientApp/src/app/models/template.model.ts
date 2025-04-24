import { Tag } from './tag.model';

export class Template {
  id: string;
  name: string;
  fileName: string;
  fileBase64: string | ArrayBuffer;
  createdDate: Date;
  updatedDate: Date;
  tags: Tag[];
}
