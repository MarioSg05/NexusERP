import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { BusinessInsightsPage } from "../../features/ai/pages/BusinessInsightsPage";

import { ProtectedRoute } from "../../features/auth/components/ProtectedRoute/ProtectedRoute";
import { RequireErpManagement } from "../../features/auth/components/RequireErpManagement/RequireErpManagement";
import { RequireUserManagement } from "../../features/auth/components/RequireUserManagement/RequireUserManagement";
import { LoginPage } from "../../features/auth/pages/LoginPage";

import { CustomersPage } from "../../features/customers/pages/CustomersPage";
import { EditCustomerPage } from "../../features/customers/pages/EditCustomerPage";
import { NewCustomerPage } from "../../features/customers/pages/NewCustomerPage";

import { DashboardPage } from "../../features/dashboard/pages/DashboardPage";

import { CreateInventoryPage } from "../../features/inventory/pages/CreateInventoryPage";
import { InventoryPage } from "../../features/inventory/pages/InventoryPage";

import { EditProductPage } from "../../features/products/pages/EditProductPage";
import { NewProductPage } from "../../features/products/pages/NewProductPage";
import { ProductsPage } from "../../features/products/pages/ProductsPage";

import { NewPurchaseOrderPage } from "../../features/purchasing/pages/NewPurchaseOrderPage";
import { PurchaseOrderDetailPage } from "../../features/purchasing/pages/PurchaseOrderDetailPage";
import { PurchasingPage } from "../../features/purchasing/pages/PurchasingPage";

import { ReportsLayout } from "../../features/reports/components/ReportsLayout/ReportsLayout";
import { InventoryReportPage } from "../../features/reports/pages/InventoryReportPage";
import { LowStockReportPage } from "../../features/reports/pages/LowStockReportPage";
import { PurchasingReportPage } from "../../features/reports/pages/PurchasingReportPage";
import { SalesReportPage } from "../../features/reports/pages/SalesReportPage";

import { NewSalesOrderPage } from "../../features/sales/pages/NewSalesOrderPage";
import { SalesOrderDetailPage } from "../../features/sales/pages/SalesOrderDetailPage";
import { SalesPage } from "../../features/sales/pages/SalesPage";

import { EditSupplierPage } from "../../features/suppliers/pages/EditSupplierPage";
import { NewSupplierPage } from "../../features/suppliers/pages/NewSupplierPage";
import { SuppliersPage } from "../../features/suppliers/pages/SuppliersPage";

import { EditUserPage } from "../../features/users/pages/EditUserPage";
import { NewUserPage } from "../../features/users/pages/NewUserPage";
import { UsersPage } from "../../features/users/pages/UsersPage";

import { RequireGuidParam } from "../../shared/components/routing/RequireGuidParam/RequireGuidParam";
import { AppLayout } from "../../shared/layouts/AppLayout";
import { NotFoundPage } from "../../shared/pages/NotFoundPage";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="login"
          element={<LoginPage />}
        />

        <Route element={<ProtectedRoute />}>
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
              path="products"
              element={<ProductsPage />}
            />

            <Route
              path="inventory"
              element={<InventoryPage />}
            />

            <Route
              path="suppliers"
              element={<SuppliersPage />}
            />

            <Route
              path="purchasing"
              element={<PurchasingPage />}
            />

            <Route
              path="sales"
              element={<SalesPage />}
            />

            <Route
              path="ai/business-insights"
              element={<BusinessInsightsPage />}
            />

            <Route
              path="reports"
              element={<ReportsLayout />}
            >
              <Route
                index
                element={
                  <Navigate
                    to="inventory"
                    replace
                  />
                }
              />

              <Route
                path="inventory"
                element={<InventoryReportPage />}
              />

              <Route
                path="low-stock"
                element={<LowStockReportPage />}
              />

              <Route
                path="sales"
                element={<SalesReportPage />}
              />

              <Route
                path="purchasing"
                element={<PurchasingReportPage />}
              />
            </Route>

            <Route element={<RequireGuidParam />}>
              <Route
                path="purchasing/:id"
                element={<PurchaseOrderDetailPage />}
              />

              <Route
                path="sales/:id"
                element={<SalesOrderDetailPage />}
              />
            </Route>

            <Route element={<RequireUserManagement />}>
              <Route
                path="users"
                element={<UsersPage />}
              />

              <Route
                path="users/new"
                element={<NewUserPage />}
              />

              <Route
                path="users/:id/edit"
                element={<EditUserPage />}
              />
            </Route>

            <Route element={<RequireErpManagement />}>
              <Route
                path="customers/new"
                element={<NewCustomerPage />}
              />

              <Route
                path="customers/:id/edit"
                element={<EditCustomerPage />}
              />

              <Route
                path="products/new"
                element={<NewProductPage />}
              />

              <Route
                path="products/:id/edit"
                element={<EditProductPage />}
              />

              <Route
                path="inventory/new"
                element={<CreateInventoryPage />}
              />

              <Route
                path="suppliers/new"
                element={<NewSupplierPage />}
              />

              <Route
                path="suppliers/:id/edit"
                element={<EditSupplierPage />}
              />

              <Route
                path="purchasing/new"
                element={<NewPurchaseOrderPage />}
              />

              <Route
                path="sales/new"
                element={<NewSalesOrderPage />}
              />
            </Route>
          </Route>
        </Route>

        <Route
          path="*"
          element={<NotFoundPage />}
        />
      </Routes>
    </BrowserRouter>
  );
}