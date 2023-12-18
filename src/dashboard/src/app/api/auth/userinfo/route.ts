import { dispatch } from '@/app/api/utils';

export async function POST(req: Request, context: { params: any }) {
  return await dispatch(`/v1/auth/userinfo`, {
    method: 'POST',
  });
}
