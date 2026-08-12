import { BrowserRouter, Route, Routes } from "react-router-dom";

import { CustomersPage } from "../../features/customers/pages/CustomersPage";
import { EditCustomerPage } from "../../features/customers/pages/EditCustomerPage";
import { NewCustomerPage } from "../../features/customers/pages/NewCustomerPage";

import { DashboardPage } from "../../features/dashboard/pages/DashboardPage";

import { NewProductPage } from "../../features/products/pages/NewProductPage";
import { EditProductPage } from "../../features/products/pages/EditProductPage";
import { ProductsPage } from "../../features/products/pages/ProductsPage";

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
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
