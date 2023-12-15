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

  const headers: any = init?.headers;
  var authorization = headers?.Authorization;
  var accept = headers?.Accept ?? 'application/json, text/plain, */*';
  var contentType = headers?.['Content-Type'] ?? 'application/json';

  if (!authorization && session) {
    if (session.user && session.accessToken) {
      const token = decrypt(session.accessToken);
      authorization = `Bearer ${token}`;
    }
  } else if (authorization === 'NA') authorization = undefined;

  console.debug(`API [${init?.method ?? 'GET'}]: ${process.env.API_URL}${input}`);
  return await fetch(`${process.env.API_URL}${input}`, {
    ...init,
    headers: {
      Accept: accept,
      'Content-Type': contentType,
      Authorization: authorization,
    },
  });
};
