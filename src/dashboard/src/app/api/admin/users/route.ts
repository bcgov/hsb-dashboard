import { dispatch } from '@/app/api/utils';

export async function GET(req: Request, context: { params: any }) {
  console.debug('params:', context);
  const url = new URL(req.url);
  return await dispatch(`/v1/admin/users${url.search}`);
}
