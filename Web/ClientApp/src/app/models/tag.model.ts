import { TagType } from "./tag-type.model";

export interface Tag {
  type: TagType;
  name: string;
  label: string;
  cellNames: string[];
  value: any;
}
