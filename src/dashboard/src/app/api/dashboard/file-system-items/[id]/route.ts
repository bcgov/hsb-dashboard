import { dispatch } from '@/app/api/utils';

export async function GET(req: Request, context: { params: any }) {
  const url = new URL(req.url);
  return await dispatch(`/v1/dashboard/file-system-items/${context.params.id}`);
}
