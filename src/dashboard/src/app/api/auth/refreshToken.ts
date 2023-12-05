import { JWT } from 'next-auth/jwt';

/**
 * Takes a token, and returns a new token with updated
 * `accessToken` and `accessTokenExpires`. If an error occurs,
 * returns the old token and an error property
 */
export async function refreshAccessToken(token: JWT) {
  try {
    const details = {
      client_id: `${process.env.KEYCLOAK_CLIENT_ID}`,
      client_secret: `${process.env.KEYCLOAK_SECRET}`,
      grant_type: ['refresh_token'],
      refresh_token: token.refresh_token,
    };
    const formBody: string[] = [];
    Object.entries(details).forEach(([key, value]: [string, any]) => {
      const encodedKey = encodeURIComponent(key);
      const encodedValue = encodeURIComponent(value);
      formBody.push(encodedKey + '=' + encodedValue);
    });
    const formData = formBody.join('&');
    const url = `${process.env.KEYCLOAK_ISSUER}${process.env.KEYCLOAK_TOKEN_URL}`;
    console.debug('Refresh URL: ', url);
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded;charset=UTF-8',
      },
      body: formData,
    });
    const refreshedToken = await response.json();
    if (!response.ok) throw refreshedToken;
    return {
      ...token,
      error: refreshedToken.error,
      access_token: refreshedToken.access_token,
      access_token_expires_at:
        Math.floor(Date.now() / 1000) + (refreshedToken.refresh_expires_in ?? 0),
      refresh_token: refreshedToken.refresh_token ?? token.refresh_token, // Fall back to old refresh token
    };
  } catch (error) {
    console.error('Refresh Token Error:', error);

    throw error;
  }
}
