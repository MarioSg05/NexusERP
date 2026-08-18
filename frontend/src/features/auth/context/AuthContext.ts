import { createContext } from "react";

import type {
  CurrentUser,
  LoginRequest,
} from "../models/AuthModel";

export interface AuthContextValue {
  user: CurrentUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  canManageErp: boolean;
  canManageUsers: boolean;
  login: (
    request: LoginRequest,
  ) => Promise<void>;
  logout: () => void;
}

export const AuthContext =
  createContext<AuthContextValue | undefined>(
    undefined,
  );