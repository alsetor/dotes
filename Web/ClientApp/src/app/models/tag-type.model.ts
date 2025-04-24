export enum TagType {
  String = 0,
  Table = 1,
  Image = 2
}

export const TagTypeDescription = new Map<number, string>([
  [TagType.String, 'String'],
  [TagType.Table, 'Table'],
  [TagType.Image, 'Image']
]);
