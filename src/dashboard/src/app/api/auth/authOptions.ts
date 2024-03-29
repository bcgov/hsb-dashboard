import { encrypt } from '@/utils';
import { jwtDecode } from 'jwt-decode';
import { Account, AuthOptions, Session, User } from 'next-auth';
import { JWT } from 'next-auth/jwt';
import KeycloakProvider from 'next-auth/providers/keycloak';
import { refreshAccessToken } from './refreshToken';

export const authOptions: AuthOptions = {
  debug: !!process.env.KEYCLOAK_DEBUG,
  secret: process.env.NEXTAUTH_SECRET,
  providers: [
    // AzureADProvider({
    //   clientId: process.env.AZURE_AD_CLIENT_ID!,
    //   clientSecret: process.env.AZURE_AD_CLIENT_SECRET!,
    //   tenantId: process.env.AZURE_AD_TENANT_ID!,
    // }),

    KeycloakProvider({
      clientId: `${process.env.KEYCLOAK_CLIENT_ID}`,
      clientSecret: `${process.env.KEYCLOAK_SECRET}`,
      issuer: process.env.KEYCLOAK_ISSUER,
      profile(profile) {
        return {
          id: profile.sub,
          name: profile.name,
          email: profile.email.toLowerCase(),
          image: null,
        };
      },
    }),
  ],
  session: {
    strategy: 'jwt',
  },

  // pages: {
  //   signIn: "/auth/signin",
  //   signOut: "/auth/signout",
  //   error: "/auth/error", // Error code passed in query string as ?error=
  //   verifyRequest: "/auth/verify-request", // (used for check email message)
  //   newUser: "/auth/new-user" // New users will be directed here on first sign in (leave the property out if not of interest)
  // },
  // pages: {
  //   signIn: '/api/auth/signin',
  //   error: '/api/auth/error',
  // },
  callbacks: {
    async jwt({
      token,
      account,
      user,
      trigger,
      session,
    }: {
      token: JWT;
      account: Account | null;
      user: User | null;
      trigger?: 'signIn' | 'update' | 'signUp' | undefined;
      session?: any;
    }) {
      const nowTimeStamp = Math.floor(Date.now() / 1000);
      const aToken = token as any;

      if (trigger === 'update') {
        const refreshToken = await refreshAccessToken(token);
        refreshToken.roles = session.user.roles;
        refreshToken.decoded = jwtDecode(`${refreshToken.access_token}`) as any;
        return refreshToken;
      } else {
        if (account && user) {
          token.access_token = account.access_token;
          token.id_token = account.id_token;
          token.expires_at = account.expires_at;
          token.refresh_token = account.refresh_token;
          const decodedToken = jwtDecode(`${account.access_token}`) as any;
          token.decoded = decodedToken;
          token.roles = decodedToken?.resource_access?.['registry-web']?.roles;
          return token;
        } else if (nowTimeStamp < aToken.expires_at) {
          // Token has not expired.
          return token;
        } else {
          const refreshToken = await refreshAccessToken(token);
          return refreshToken;
        }
      }
    },
    async signIn({ user, account }) {
      if (account && user) {
        return true;
      } else {
        // TODO : Add unauthorized page
        return '/unauthorized';
      }
    },
    async session({ session, token }: { session: Session; token: JWT }) {
      // Send properties to the client, like an access_token from a provider.
      if (token) {
        session.idToken = encrypt(`${token.id_token}`);
        session.accessToken = encrypt(`${token.access_token}`);
        session.user.roles = token.roles ?? token.decoded?.client_roles ?? [];
      }

      return session;
    },
    redirect({ baseUrl }) {
      return baseUrl;
    },
  },
  events: {
    signOut: ({ session, token }) => {
      console.debug('signOut');
    },
  },
};
