export const GET = async (req: Request, res: Response) => {
  return Response.json({ message: 'running' }, { status: 200 });
};
