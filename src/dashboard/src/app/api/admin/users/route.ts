import { dispatch } from '@/app/api/utils';

export async function GET(req: Request, context: { params: any }) {
  const url = new URL(req.url);
  return await dispatch(`/v1/admin/users${url.search}`);
}

export async function POST(req: Request, context: { params: any }) {
  const body = await req.json();
  return await dispatch(`/v1/admin/users`, {
    method: 'POST',
    body: JSON.stringify(body),
  });
}
