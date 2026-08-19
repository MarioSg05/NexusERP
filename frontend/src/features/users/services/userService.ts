import { apiClient } from "../../../shared/api/client";

import type { User } from "../models/UserModel";

import type {
  RegisterUserRequest,
  RegisterUserResponse,
} from "../models/RegisterUserModel";

import type {
  UpdateUserRequest,
  UpdateUserResponse,
} from "../models/UpdateUserModel";

import type {
  ChangeUserRoleRequest,
  ChangeUserRoleResponse,
} from "../models/ChangeUserRoleModel";

import type { UserStatusResponse } from "../models/UserStatusModel";

export async function getUsers(): Promise<
  User[]
> {
  const response =
    await apiClient.get<User[]>("/users");

  return response.data;
}

export async function getUserById(
  id: string,
): Promise<User> {
  const response =
    await apiClient.get<User>(
      `/users/${id}`,
    );

  return response.data;
}

export async function registerUser(
  request: RegisterUserRequest,
): Promise<RegisterUserResponse> {
  const response =
    await apiClient.post<RegisterUserResponse>(
      "/users",
      request,
    );

  return response.data;
}

export async function updateUser(
  id: string,
  request: UpdateUserRequest,
): Promise<UpdateUserResponse> {
  const response =
    await apiClient.put<UpdateUserResponse>(
      `/users/${id}`,
      request,
    );

  return response.data;
}

export async function changeUserRole(
  id: string,
  request: ChangeUserRoleRequest,
): Promise<ChangeUserRoleResponse> {
  const response =
    await apiClient.put<ChangeUserRoleResponse>(
      `/users/${id}/role`,
      request,
    );

  return response.data;
}

export async function activateUser(
  id: string,
): Promise<UserStatusResponse> {
  const response =
    await apiClient.post<UserStatusResponse>(
      `/users/${id}/activate`,
    );

  return response.data;
}

export async function deactivateUser(
  id: string,
): Promise<UserStatusResponse> {
  const response =
    await apiClient.post<UserStatusResponse>(
      `/users/${id}/deactivate`,
    );

  return response.data;
}