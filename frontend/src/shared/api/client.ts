import axios from "axios";

import {
  clearAuthSession,
  getAccessToken,
} from "../../features/auth/services/authSession";

import { notifyUnauthorized } from "../../features/auth/services/authEvents";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL?.trim();

if (!apiBaseUrl) {
  throw new Error(
    "Missing VITE_API_BASE_URL. Configure it in frontend/.env.local.",
  );
}

export const apiClient = axios.create({
  baseURL: apiBaseUrl,

  headers: {
    "Content-Type": "application/json",
  },

  timeout: 10000,
});

apiClient.interceptors.request.use(
  (config) => {
    const accessToken =
      getAccessToken();

    if (accessToken) {
      config.headers.Authorization =
        `Bearer ${accessToken}`;
    }

    return config;
  },
);

apiClient.interceptors.response.use(
  (response) => response,
  (error: unknown) => {
    if (
      axios.isAxiosError(error) &&
      error.response?.status === 401
    ) {
      const hadSession =
        getAccessToken() !== null;

      if (hadSession) {
        clearAuthSession();
        notifyUnauthorized();
      }
    }

    return Promise.reject(error);
  },
);