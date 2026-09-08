import {
  Navigate,
  Outlet,
  useLocation,
} from "react-router-dom";

import { useAuth } from "../../hooks/useAuth";

function getFallbackPath(
  pathname: string,
): string {
  if (pathname.startsWith("/customers")) {
    return "/customers";
  }

  if (pathname.startsWith("/products")) {
    return "/products";
  }

  if (pathname.startsWith("/inventory")) {
    return "/inventory";
  }

  if (pathname.startsWith("/purchasing")) {
    return "/purchasing";
  }

  if (pathname.startsWith("/sales")) {
    return "/sales";
  }

  return "/";
}

export function RequireErpManagement() {
  const { canManageErp } = useAuth();
  const location = useLocation();

  if (!canManageErp) {
    return (
      <Navigate
        to={getFallbackPath(
          location.pathname,
        )}
        replace
      />
    );
  }

  return <Outlet />;
}