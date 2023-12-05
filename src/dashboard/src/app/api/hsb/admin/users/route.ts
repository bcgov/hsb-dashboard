import { dispatch } from '@/app/api/utils';

export const GET = async (req: Request, res: Response) => {
  if (req.method === 'GET') {
    const url = new URL(req.url);
    return await dispatch(`/v1/system/admin/users${url.search}`);
  }

  return Response.json({ message: 'Invalid HTTP method' }, { status: 405 });
};
