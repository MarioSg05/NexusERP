import type { UserRole } from "../../auth/models/UserRole";

export interface RegisterUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: UserRole;
}

export interface RegisterUserResponse {
  id: string;
  email: string;
  role: UserRole;
}