'use client';

import { Button } from '@/components';

interface IPageProps {
  params: { id: string; searchParams: any };
}

export default function Page({ params }: IPageProps) {
  return (
    <div>
      {params.id}:{params.searchParams}
      <Button onClick={() => {}}>Click me</Button>
    </div>
  );
}
