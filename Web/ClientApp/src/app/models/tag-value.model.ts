import { TagType } from "./tag-type.model";

export interface TagValue {
  type: TagType;
  name: string;
  value: string;
}
