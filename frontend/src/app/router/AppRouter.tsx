import { BrowserRouter } from "react-router-dom";

import { AppLayout } from "../../shared/layouts/AppLayout";

export function AppRouter() {
  return (
    <BrowserRouter>
      <AppLayout />
    </BrowserRouter>
  );
}