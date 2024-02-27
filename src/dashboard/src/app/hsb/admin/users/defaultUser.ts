import { IUserForm } from '@/components/admin';
import moment from 'moment';

export const defaultUser = (): IUserForm => {
  return {
    id: 0,
    key: crypto.randomUUID(),
    username: '',
    email: '',
    emailVerified: false,
    displayName: '',
    firstName: '',
    middleName: '',
    lastName: '',
    phone: '',
    isEnabled: false,
    failedLogins: 0,
    note: '',
    preferences: {},
    createdOn: moment().toISOString(),
    createdBy: '',
    updatedOn: moment().toISOString(),
    updatedBy: '',
    version: 0,
  };
};
