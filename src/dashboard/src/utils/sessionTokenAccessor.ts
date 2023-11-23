import { authOptions } from '@/app/api/auth';
import { getServerSession } from 'next-auth';
import { decrypt } from '.';

export const getAccessToken = async () => {
  const session = await getServerSession(authOptions);
  if (session) {
    const aSession = session as any;
    const result = decrypt(aSession.access_token);
    return result;
  }
  return null;
};

export const getIdToken = async () => {
  const session = await getServerSession(authOptions);
  if (session) {
    const aSession = session as any;
    const result = decrypt(aSession.id_token);
    return result;
  }
  return null;
};
