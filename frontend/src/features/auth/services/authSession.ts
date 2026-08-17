const ACCESS_TOKEN_KEY =
  "nexuserp.accessToken";

const EXPIRES_AT_KEY =
  "nexuserp.expiresAt";

export function saveAuthSession(
  accessToken: string,
  expiresAt: string,
) {
  sessionStorage.setItem(
    ACCESS_TOKEN_KEY,
    accessToken,
  );

  sessionStorage.setItem(
    EXPIRES_AT_KEY,
    expiresAt,
  );
}

export function getAccessToken() {
  return sessionStorage.getItem(
    ACCESS_TOKEN_KEY,
  );
}

export function getExpiresAt() {
  return sessionStorage.getItem(
    EXPIRES_AT_KEY,
  );
}

export function clearAuthSession() {
  sessionStorage.removeItem(
    ACCESS_TOKEN_KEY,
  );

  sessionStorage.removeItem(
    EXPIRES_AT_KEY,
  );
}

export function hasValidAuthSession() {
  const accessToken = getAccessToken();
  const expiresAt = getExpiresAt();

  if (!accessToken || !expiresAt) {
    return false;
  }

  const expiration =
    new Date(expiresAt).getTime();

  if (
    Number.isNaN(expiration) ||
    expiration <= Date.now()
  ) {
    clearAuthSession();

    return false;
  }

  return true;
}