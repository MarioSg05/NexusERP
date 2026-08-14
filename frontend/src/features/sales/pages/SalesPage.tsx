import { SalesEmptyState } from "../components/SalesEmptyState/SalesEmptyState";
import { SalesHeader } from "../components/SalesHeader/SalesHeader";
import { SalesOrdersTable } from "../components/SalesOrdersTable/SalesOrdersTable";
import { SalesSkeleton } from "../components/SalesSkeleton/SalesSkeleton";
import { useSalesOrders } from "../hooks/useSalesOrders";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function SalesPage() {
  const {
    data: salesOrders,
    isLoading,
    error,
    refetch,
  } = useSalesOrders();

  if (isLoading) {
    return <SalesSkeleton />;
  }

  if (error || !salesOrders) {
    return (
      <QueryErrorState
        title="Unable to load sales orders"
        description="We couldn't retrieve the sales information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <SalesHeader />

      <div className="mt-8">
        {salesOrders.length === 0 ? (
          <SalesEmptyState />
        ) : (
          <SalesOrdersTable
            salesOrders={salesOrders}
          />
        )}
      </div>
    </>
  );
} 