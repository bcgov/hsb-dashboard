import { decrypt } from '@/utils';
import { getServerSession } from 'next-auth';
import { authOptions } from '../auth';

/**
 * Dispatch an AJAX fetch request to the API.
 * Applies the JWT to the request.
 * @param input RequestInfo
 * @param init RequestInit | undefined
 * @returns Promise with response
 */
export const dispatch = async (
  input: RequestInfo,
  init?: RequestInit | undefined,
): Promise<Response> => {
  const session = await getServerSession(authOptions);

  if (session) {
    if (session.user && session.accessToken) {
      const token = decrypt(session.accessToken);
      return await fetch(`${process.env.API_URL}${input}`, {
        ...init,
        headers: {
          Accept: 'application/json, text/plain, */*',
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return Response.json({ message: 'Not authorized' }, { status: 403 });
  }

  return Response.json({ message: 'Not authenticated' }, { status: 401 });
};
