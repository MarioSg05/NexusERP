import { BrowserRouter, Route, Routes } from "react-router-dom";

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

import { AppLayout } from "../../shared/layouts/AppLayout";

export function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<AppLayout />}>
                    <Route index element={<DashboardPage />} />

                    <Route path="customers" element={<CustomersPage />} />

                    <Route path="customers/new" element={<NewCustomerPage />} />

                    <Route
                        path="customers/:id/edit"
                        element={<EditCustomerPage />}
                    />

                    <Route path="products" element={<ProductsPage />} />

                    <Route path="products/new" element={<NewProductPage />} />

                    <Route
                        path="products/:id/edit"
                        element={<EditProductPage />}
                    />

                    <Route path="inventory" element={<InventoryPage />} />

                    <Route
                        path="inventory/new"
                        element={<CreateInventoryPage />}
                    />

                    <Route path="purchasing" element={<PurchasingPage />} />

                    <Route
                        path="purchasing/new"
                        element={<NewPurchaseOrderPage />}
                    />

                    <Route
                        path="purchasing/:id"
                        element={<PurchaseOrderDetailPage />}
                    />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
