import { dispatch } from '@/app/api/utils';

export async function GET(req: Request, context: { params: any }) {
  const url = new URL(req.url);
  return await dispatch(`/v1/admin/users/${context.params.id}${url.search}`);
}

export async function PUT(req: Request, context: { params: any }) {
  const body = await req.json();
  return await dispatch(`/v1/admin/users/${context.params.id}`, {
    method: 'PUT',
    body: JSON.stringify(body),
  });
}
