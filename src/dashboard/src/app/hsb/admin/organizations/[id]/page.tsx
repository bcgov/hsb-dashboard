'use client';

import { Button } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

interface IPageProps {
  params: { id: string; searchParams: any };
}

export default function Page({ params }: IPageProps) {
  const state = useAuth();

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <div>
      {params.id}:{params.searchParams}
      <Button onClick={() => {}}>Click me</Button>
    </div>
  );
}
