import { useEffect, useState } from "react";

import type { Dashboard } from "../models/DashboardModel";
import { getDashboard } from "../services/dashboardService";

import { DashboardKpiCard } from "../components/DashboardKpiCard/DashboardKpiCard";
import { DashboardSummaryCard } from "../components/DashboardSummaryCard/DashboardSummaryCard";
import { DashboardHeader } from "../components/DashboardHeader/DashboardHeader";

export function DashboardPage() {
    const [dashboard, setDashboard] = useState<Dashboard | null>(null);

    useEffect(() => {
        async function loadDashboard() {
            const data = await getDashboard();

            setDashboard(data);
        }

        loadDashboard();
    }, []);

    if (!dashboard) {
        return <p>Loading dashboard...</p>;
    }

    return (
        <>
            <DashboardHeader />

            <section className="grid grid-cols-3 gap-6">
                <DashboardKpiCard
                    title="Products"
                    value={dashboard.inventory.totalProducts}
                />

                <DashboardKpiCard
                    title="Sales"
                    value={dashboard.sales.totalSalesOrders}
                />

                <DashboardKpiCard
                    title="Purchasing"
                    value={dashboard.purchasing.totalPurchaseOrders}
                />
            </section>

            <div className="mt-8 grid grid-cols-3 gap-6">
                <DashboardSummaryCard
                    title="Inventory"
                    items={[
                        {
                            label: "Total Products",
                            value: dashboard.inventory.totalProducts,
                        },
                        {
                            label: "Active Products",
                            value: dashboard.inventory.activeProducts,
                        },
                        {
                            label: "Low Stock",
                            value: dashboard.inventory.lowStockProducts,
                        },
                    ]}
                />

                <DashboardSummaryCard
                    title="Sales"
                    items={[
                        {
                            label: "Total Orders",
                            value: dashboard.sales.totalSalesOrders,
                        },
                        {
                            label: "Pending Orders",
                            value: dashboard.sales.pendingSalesOrders,
                        },
                        {
                            label: "Total Sales",
                            value: dashboard.sales.totalSalesAmount,
                        },
                    ]}
                />

                <DashboardSummaryCard
                    title="Purchasing"
                    items={[
                        {
                            label: "Total Orders",
                            value: dashboard.purchasing.totalPurchaseOrders,
                        },
                        {
                            label: "Pending Orders",
                            value: dashboard.purchasing.pendingPurchaseOrders,
                        },
                        {
                            label: "Total Purchasing",
                            value: dashboard.purchasing.totalPurchasingAmount,
                        },
                    ]}
                />
            </div>
        </>
    );
}
