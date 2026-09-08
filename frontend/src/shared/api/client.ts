import axios from "axios";

import {
  clearAuthSession,
  getAccessToken,
} from "../../features/auth/services/authSession";

import { notifyUnauthorized } from "../../features/auth/services/authEvents";

export const apiClient = axios.create({
  baseURL: "http://localhost:5012/api",

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