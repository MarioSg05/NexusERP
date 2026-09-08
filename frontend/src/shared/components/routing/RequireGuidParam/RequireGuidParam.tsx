import {
  Outlet,
  useParams,
} from "react-router-dom";

import { NotFoundPage } from "../../../pages/NotFoundPage";

const guidPattern =
  /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

interface RequireGuidParamProps {
  paramName?: string;
}

export function RequireGuidParam({
  paramName = "id",
}: RequireGuidParamProps) {
  const params = useParams();

  const value = params[paramName];

  if (
    !value ||
    !guidPattern.test(value)
  ) {
    return <NotFoundPage />;
  }

  return <Outlet />;
}