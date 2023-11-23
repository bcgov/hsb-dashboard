import { getIdToken } from '@/utils';
import { getServerSession } from 'next-auth';
import { authOptions } from '..';

export const POST = async (req: Request, res: Response) => {
  const session = await getServerSession(authOptions);
  if (session) {
    try {
      const idToken = await getIdToken();
      var url = `${process.env.KEYCLOAK_ISSUER}${process.env.KEYCLOAK_END_SESSION_PATH}?id_token_hint=${idToken}`;
      await fetch(url, { method: 'GET' });
    } catch (error) {
      console.error('logout error:', error);
      return new Response(null, { status: 500 });
    }
  }
  return new Response(null, { status: 200 });
};
