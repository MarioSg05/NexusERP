import {
  Navigate,
  Outlet,
} from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";

export function RequireUserManagement() {
  const { canManageUsers } = useAuth();

  if (!canManageUsers) {
    return (
      <Navigate
        to="/"
        replace
      />
    );
  }

  return <Outlet />;
}