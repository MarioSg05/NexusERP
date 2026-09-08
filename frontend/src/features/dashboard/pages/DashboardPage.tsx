import { DashboardHeader } from "../components/DashboardHeader/DashboardHeader";
import { DashboardKpiCard } from "../components/DashboardKpiCard/DashboardKpiCard";
import { DashboardSkeleton } from "../components/DashboardSkeleton/DashboardSkeleton";
import { DashboardSummaryCard } from "../components/DashboardSummaryCard/DashboardSummaryCard";
import { useDashboard } from "../hooks/useDashboard";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function DashboardPage() {
  const {
    data: dashboard,
    isLoading,
    error,
    refetch,
  } = useDashboard();

  if (isLoading) {
    return <DashboardSkeleton />;
  }

  if (error || !dashboard) {
    return (
      <QueryErrorState
        title="Unable to load dashboard"
        description="We couldn't retrieve the latest dashboard information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <DashboardHeader />

      <section className="grid grid-cols-1 gap-6 md:grid-cols-2 xl:grid-cols-3">
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

      <div className="mt-8 space-y-6">
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