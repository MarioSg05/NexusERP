import {
  BrowserRouter,
  Route,
  Routes,
} from "react-router-dom";

import { CustomersPage } from "../../features/customers/pages/CustomersPage";
import { EditCustomerPage } from "../../features/customers/pages/EditCustomerPage";
import { NewCustomerPage } from "../../features/customers/pages/NewCustomerPage";
import { DashboardPage } from "../../features/dashboard/pages/DashboardPage";
import { AppLayout } from "../../shared/layouts/AppLayout";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          <Route
            index
            element={<DashboardPage />}
          />

          <Route
            path="customers"
            element={<CustomersPage />}
          />

          <Route
            path="customers/new"
            element={<NewCustomerPage />}
          />

          <Route
            path="customers/:id/edit"
            element={<EditCustomerPage />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}