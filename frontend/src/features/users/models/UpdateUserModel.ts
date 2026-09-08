import type { UserRole } from "../../auth/models/UserRole";

export interface UpdateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
}

export interface UpdateUserResponse {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  isActive: boolean;
}