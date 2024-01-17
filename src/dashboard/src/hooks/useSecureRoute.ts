import { redirect } from 'next/navigation';
import { useLayoutEffect } from 'react';
import { IAuthState, useAuth } from '.';

/**
 * Hook to redirect a user to another route if they don't pass the `predicate`.
 * @param predicate Function to validate the current user can access the current route.
 * @param redirectTo If user is not allowed on current route, then `redirectTo`.
 */
export const useSecureRoute = (predicate: (state: IAuthState) => boolean, redirectTo: string) => {
  const state = useAuth();

  useLayoutEffect(() => {
    if (!predicate(state)) redirect(redirectTo);
  }, [predicate, redirectTo, state]);
};
