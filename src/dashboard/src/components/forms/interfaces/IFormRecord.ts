export interface IFormRecord {
  isDirty?: boolean;
  isValid?: boolean;
  errors?: { [name: string]: string };
  touched?: { [name: string]: boolean };
}
