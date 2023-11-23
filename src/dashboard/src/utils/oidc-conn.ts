import getConfig from 'next/config';
import OIDC from './oidc';

const { serverRuntimeConfig = {} } = getConfig() || {};
const {
  sso_configuration_url,
  sso_url,
  sso_client_id,
  sso_client_secret,
  sso_redirect_uri,
  sso_logout_redirect_uri,
  sso_authorization_response_type,
  sso_authorization_scope,
  sso_token_grant_type,
} = serverRuntimeConfig;

let _oidc: any;
export const createOIDC = () => {
  if (_oidc) return _oidc;

  _oidc = new OIDC({
    configurationUrl: sso_configuration_url,
    url: sso_url,
    clientId: sso_client_id,
    clientSecret: sso_client_secret,
    redirectUri: sso_redirect_uri,
    logoutRedirectUri: sso_logout_redirect_uri,
    authResponseType: sso_authorization_response_type,
    authScope: sso_authorization_scope,
    tokenGrantType: sso_token_grant_type,
  });

  return _oidc;
};
