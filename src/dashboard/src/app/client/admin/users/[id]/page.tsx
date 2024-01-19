'use client';

import { Button } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

interface IPageProps {
  params: { id: string; searchParams: any };
}

export default function Page({ params }: IPageProps) {
  const state = useAuth();

  // Only allow Organization Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isOrganizationAdmin) redirect('/');

  return (
    <div>
      {params.id}:{params.searchParams}
      <Button onClick={() => {}}>Click me</Button>
    </div>
  );
}
