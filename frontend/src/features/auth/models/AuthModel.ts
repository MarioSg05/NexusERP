export type UserRole =
  | "Administrator"
  | "Manager"
  | "Viewer";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  email: string;
  accessToken: string;
  expiresAt: string;
}

export interface CurrentUser {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  isActive: boolean;
}