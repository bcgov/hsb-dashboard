export interface IFormRecord {
  isDirty?: boolean;
  isValid?: boolean;
  isSubmitting?: boolean;
  errors?: { [name: string]: string };
  touched?: { [name: string]: boolean };
}
