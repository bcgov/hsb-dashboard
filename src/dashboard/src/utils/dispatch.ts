import { signOut } from 'next-auth/react';

export const dispatch = async (
  input: string | Request | URL,
  init?: RequestInit | undefined,
): Promise<Response> => {
  const response = await fetch(input, init);
  // If they are unauthenticated when making requests to the API then their token is expired.
  if (response.status === 401) signOut();

  // Throw an error so that it bubbles out.
  if (!response.ok) {
    let message = response.statusText;

    try {
      const error = await response.json();
      if (!!error.error) message = error.error;
    } catch {}

    throw new Error(`${response.status}: ${message}`);
  }
  return response;
};
