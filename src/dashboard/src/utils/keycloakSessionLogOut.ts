import { signOut } from 'next-auth/react';
import { toast } from 'react-toastify';

export const keycloakSessionLogOut = async () => {
  try {
    const res = await fetch('/api/auth/logout', { method: 'POST' });
    const body = await res.text();
    if (res.status === 500) throw new Error(body);
    await signOut({ redirect: true, callbackUrl: '/' });
  } catch (ex) {
    const error = ex as Error;
    toast.error(error.message);
    console.error(error);
  }
};
