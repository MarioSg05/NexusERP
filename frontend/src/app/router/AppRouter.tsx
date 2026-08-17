import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { LoginPage } from "../../features/auth/pages/LoginPage";
import { ProtectedRoute } from "../../features/auth/components/ProtectedRoute/ProtectedRoute";

import { CustomersPage } from "../../features/customers/pages/CustomersPage";
import { EditCustomerPage } from "../../features/customers/pages/EditCustomerPage";
import { NewCustomerPage } from "../../features/customers/pages/NewCustomerPage";

import { DashboardPage } from "../../features/dashboard/pages/DashboardPage";

import { NewProductPage } from "../../features/products/pages/NewProductPage";
import { EditProductPage } from "../../features/products/pages/EditProductPage";
import { ProductsPage } from "../../features/products/pages/ProductsPage";

import { InventoryPage } from "../../features/inventory/pages/InventoryPage";
import { CreateInventoryPage } from "../../features/inventory/pages/CreateInventoryPage";

import { PurchasingPage } from "../../features/purchasing/pages/PurchasingPage";
import { PurchaseOrderDetailPage } from "../../features/purchasing/pages/PurchaseOrderDetailPage";
import { NewPurchaseOrderPage } from "../../features/purchasing/pages/NewPurchaseOrderPage";

import { SalesPage } from "../../features/sales/pages/SalesPage";
import { SalesOrderDetailPage } from "../../features/sales/pages/SalesOrderDetailPage";
import { NewSalesOrderPage } from "../../features/sales/pages/NewSalesOrderPage";

import { ReportsLayout } from "../../features/reports/components/ReportsLayout/ReportsLayout";
import { InventoryReportPage } from "../../features/reports/pages/InventoryReportPage";
import { LowStockReportPage } from "../../features/reports/pages/LowStockReportPage";
import { PurchasingReportPage } from "../../features/reports/pages/PurchasingReportPage";
import { SalesReportPage } from "../../features/reports/pages/SalesReportPage";

import { AppLayout } from "../../shared/layouts/AppLayout";

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
              path="customers/new"
              element={<NewCustomerPage />}
            />

            <Route
              path="customers/:id/edit"
              element={<EditCustomerPage />}
            />

            <Route
              path="products"
              element={<ProductsPage />}
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
              path="inventory"
              element={<InventoryPage />}
            />

            <Route
              path="inventory/new"
              element={<CreateInventoryPage />}
            />

            <Route
              path="purchasing"
              element={<PurchasingPage />}
            />

            <Route
              path="purchasing/new"
              element={<NewPurchaseOrderPage />}
            />

            <Route
              path="purchasing/:id"
              element={<PurchaseOrderDetailPage />}
            />

            <Route
              path="sales"
              element={<SalesPage />}
            />

            <Route
              path="sales/new"
              element={<NewSalesOrderPage />}
            />

            <Route
              path="sales/:id"
              element={<SalesOrderDetailPage />}
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
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}