'use client';

import { Button } from '@/components';
import { Checkbox } from '@/components/forms/checkbox';
import { Select } from '@/components/forms/select';
import { signIn } from 'next-auth/react';
import { FaSignInAlt } from 'react-icons/fa';

export default function Page() {
  return (
    <div className="container">
      <div className="panel">
        <div>
          <h1>Welcome to the Storage Dashboard Style Guide</h1>
        </div>
        <div>
          <h2>Buttons</h2>
          <Button onClick={() => signIn('keycloak')} title="Sign in" variant="primary">
            Button primary
          </Button>
        </div>
        <div>
          <h2>Checkbox</h2>
          <Checkbox></Checkbox>
        </div>
        <div>
          <h2>Select</h2>
          <Select title="dropdown example"></Select>
        </div>
      </div>
    </div>
  );
}
