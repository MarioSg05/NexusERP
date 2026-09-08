import type { UserRole } from "../../auth/models/UserRole";

export interface ChangeUserRoleRequest {
  role: UserRole;
}

export interface ChangeUserRoleResponse {
  id: string;
  role: UserRole;
}