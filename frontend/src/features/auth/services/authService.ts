import { apiClient } from "../../../shared/api/client";

import type {
  CurrentUser,
  LoginRequest,
  LoginResponse,
} from "../models/AuthModel";

export async function login(
  request: LoginRequest,
): Promise<LoginResponse> {
  const response =
    await apiClient.post<LoginResponse>(
      "/auth/login",
      request,
    );

  return response.data;
}

export async function getCurrentUser(): Promise<CurrentUser> {
  const response =
    await apiClient.get<CurrentUser>(
      "/auth/me",
    );

  return response.data;
}