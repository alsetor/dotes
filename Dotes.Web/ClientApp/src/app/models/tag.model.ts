import { TagType } from "./tag-type.model";

export interface Tag {
  type: TagType;
  name: string;
  displayName: string;
  cellNames: string[];
  value: any;
}
