import { IUserForm } from '@/components/admin';
import { validateEmail } from '@/utils';

export const validateUser = (
  values: IUserForm,
  setErrors: React.Dispatch<
    React.SetStateAction<{ [key: string]: { [K in keyof IUserForm]?: string } }>
  >,
) => {
  const updatedErrors: { [K in keyof IUserForm]?: string } = {};
  if (!values.username) updatedErrors.username = 'required';
  if (!values.email) updatedErrors.email = 'required';
  else if (!validateEmail(values.email)) updatedErrors.email = 'invalid email';
  if (!values.displayName) updatedErrors.displayName = 'required';

  setErrors((errors) => ({ ...errors, [values.key]: updatedErrors }));

  return !Object.keys(updatedErrors).length;
};
