import axios, { AxiosRequestConfig } from 'axios';
import jwt from 'jsonwebtoken';
import jwkToPem, { JWK } from 'jwk-to-pem';
import jws from 'jws';
import qs from 'qs';

type _JWK = { kid: string } & JWK;

const btoa = (str: string) => Buffer.from(str).toString('base64');

const getUrlQuerySeparator = (url: string) => (url.includes('?') ? '&' : '?');

interface OID_PROVIDER_CONFIGURATION {
  issuer: string;
  jwks_uri: string;
  authorization_endpoint: string;
  token_endpoint: string;
  userinfo_endpoint: string;
  end_session_endpoint: string;
  jwks: _JWK[];
}

interface TOKEN_RESPONSE {
  id_token?: string;
  access_token?: string;
  refresh_token?: string;
  expires_in?: number;
  token_type?: string;
}

interface AUTHORIZATION_REQUEST_PARAMETERS {
  client_id: string;
  response_type: string; // 'code' | 'id_token token' | 'code id_token token'
  scope: string;
  redirect_uri: string;
  response_mode?: string; // 'query' | 'fragment'
  nonce?: string;
}

interface END_SESSION_PARAMETERS {
  redirect_uri: string;
}

interface Props {
  configurationUrl?: string;
  url?: string;
  clientId: string;
  clientSecret: string;
  redirectUri: string;
  logoutRedirectUri?: string;
  authResponseType: string;
  authScope: string;
  // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.3
  // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.3.2
  // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.4.2
  // see https://datatracker.ietf.org/doc/html/rfc6749#section-6
  tokenGrantType: string; // 'authorization_code (Standard Flow)' | 'password (Direct Access Grants)' | 'client_credentials (Client Authentication)' | 'refresh_token'
}

interface ISignatureToken extends jws.Signature {
  header: any;
}

class OIDC {
  private _providerConfigurationUrl: string;
  private _configurationUrl?: string;
  private _url?: string;
  private _clientId: string;
  private _clientSecret: string;
  private _redirectUri: string;
  private _logoutRedirectUri: string;
  private _authResponseType: string;
  private _authScope: string;
  private _tokenGrantType: string;
  private _confidential: boolean;
  private _providerConfiguration: OID_PROVIDER_CONFIGURATION = {
    issuer: '',
    jwks_uri: '',
    authorization_endpoint: '',
    token_endpoint: '',
    userinfo_endpoint: '',
    end_session_endpoint: '',
    jwks: [],
  };

  constructor({
    configurationUrl,
    url,
    clientId,
    clientSecret,
    redirectUri,
    logoutRedirectUri,
    authResponseType,
    authScope,
    tokenGrantType,
  }: Props) {
    this._configurationUrl = configurationUrl;
    this._url = url;
    this._clientId = clientId;
    this._clientSecret = clientSecret;
    this._redirectUri = redirectUri;
    this._logoutRedirectUri = logoutRedirectUri || redirectUri;
    this._authResponseType = authResponseType;
    this._authScope = authScope;
    this._tokenGrantType = tokenGrantType;
    this._confidential = !!clientSecret;
    this._providerConfigurationUrl =
      this._configurationUrl || `${this._url}/.well-known/openid-configuration`;
  }

  get providerConfiguration() {
    return this._providerConfiguration;
  }

  // https://github.com/vercel/next.js/discussions/15341
  // https://flaviocopes.com/nextjs-cache-data-globally/
  private async fetchIssuerConfiguration() {
    if (this._providerConfiguration?.issuer) return this._providerConfiguration;

    const {
      issuer,
      jwks_uri,
      authorization_endpoint,
      token_endpoint,
      userinfo_endpoint,
      end_session_endpoint,
    } = await axios
      .get(this._providerConfigurationUrl)
      .then((res: { data: any }) => res.data, console.error);

    const jwks = await axios.get(jwks_uri).then((res: any) => res.data?.keys, console.error);

    this._providerConfiguration = {
      issuer,
      jwks_uri,
      authorization_endpoint,
      token_endpoint,
      userinfo_endpoint,
      end_session_endpoint,
      jwks,
    };

    return this._providerConfiguration;
  }

  // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.1
  public async getAuthorizationUrl(extraParams = {}) {
    const providerConfig = await this.fetchIssuerConfiguration();
    const params: AUTHORIZATION_REQUEST_PARAMETERS = {
      client_id: this._clientId,
      response_type: this._authResponseType,
      scope: this._authScope,
      redirect_uri: this._redirectUri,
      ...extraParams,
    };

    if (params.response_type === 'code') {
      params.response_mode = 'query';
    } else {
      params.response_mode = 'fragment';

      if (!params.nonce) {
        throw Error('Missing parameter "nonce"');
      }

      // Example using response_type=id_token token
      // http://localhost:3000/oidc/keycloak
      // #session_state=xxxxxxxx-7af1-40e9-bc2f-xxxxxxxxxxxx
      // &id_token=hhhhh.pppppppppp.fffff
      // &access_token=hhhhh.pppppppppp.fffff
      // &token_type=bearer
      // &expires_in=900
    }

    // TODO: let's apply PKCE workflow for public clients
    if (!this._confidential) {
    }

    const separator = getUrlQuerySeparator(providerConfig?.authorization_endpoint);
    return `${providerConfig?.authorization_endpoint}${separator}${qs.stringify(params, {
      encode: false,
    })}`;
  }

  public async getEndSessionUrl(extraParams = {}) {
    const providerConfig = await this.fetchIssuerConfiguration();
    const params: END_SESSION_PARAMETERS = {
      redirect_uri: this._logoutRedirectUri,
      ...extraParams,
    };

    const separator = getUrlQuerySeparator(providerConfig?.end_session_endpoint);
    return `${providerConfig?.end_session_endpoint}${separator}${qs.stringify(params, {
      encode: false,
    })}`;
  }

  // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.3
  // see https://aws.amazon.com/blogs/mobile/understanding-amazon-cognito-user-pool-oauth-2-0-grants/
  public async getAccessToken({ code }: { code: string }) {
    const providerConfig = await this.fetchIssuerConfiguration();
    const params = {
      grant_type: this._tokenGrantType,
      client_id: this._clientId,
      redirect_uri: this._redirectUri,
      code,
    };

    // TODO: let's apply PKCE workflow for public clients
    if (!this._confidential) {
    }

    // see https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
    // see https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.4
    // Oauth2 response + OpenID Connect spec; id_token
    // {
    //   id_token: "xxxxxxx...",
    //   access_token: "xxxxxxx...",
    //   refresh_token: "xxxxxxx...",
    //   expires_in: 3600,
    //   token_type: "Bearer",
    // };
    const config: AxiosRequestConfig = {
      url: providerConfig?.token_endpoint,
      method: 'post',
      data: qs.stringify(params),
    };

    if (this._confidential) {
      config.headers = {
        Authorization: `Basic ${btoa(`${this._clientId}:${this._clientSecret}`)}`,
      };
    }

    const { data } = await axios(config);
    return data as TOKEN_RESPONSE;
  }

  public async getUserInfo({ accessToken }: { accessToken: string }) {
    const providerConfig = await this.fetchIssuerConfiguration();

    const config: AxiosRequestConfig = {
      url: providerConfig?.userinfo_endpoint,
      method: 'get',
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    };

    const { data } = await axios(config);
    return data as any;
  }

  public async refreshSession({ refreshToken }: { refreshToken: string }) {
    const providerConfig = await this.fetchIssuerConfiguration();

    const params = {
      grant_type: 'refresh_token',
      client_id: this._clientId,
      refresh_token: refreshToken,
    };

    const config: AxiosRequestConfig = {
      url: providerConfig?.token_endpoint,
      method: 'post',
      data: qs.stringify(params),
    };

    const { data } = await axios(config);
    return data as any;
  }

  public async verifyToken(token: string) {
    // 1. Decode the ID token.
    const { header } = jws.decode(token) as ISignatureToken;

    // 2. Compare the local key ID (kid) to the public kid.
    const { jwks, issuer } = await this.fetchIssuerConfiguration();

    const key = jwks.find((key) => key.kid === header.kid);

    if (!key) {
      return false;
    }

    // 3. Verify the signature using the public key
    const pem = jwkToPem(key);

    return jwt.verify(token, pem, {
      audience: this._clientId,
      issuer,
      maxAge: '4h',
    });
  }
}

export default OIDC;
