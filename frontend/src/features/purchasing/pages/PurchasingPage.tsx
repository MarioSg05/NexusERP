import { PurchaseOrdersTable } from "../components/PurchaseOrdersTable/PurchaseOrdersTable";
import { PurchasingEmptyState } from "../components/PurchasingEmptyState/PurchasingEmptyState";
import { PurchasingHeader } from "../components/PurchasingHeader/PurchasingHeader";
import { PurchasingSkeleton } from "../components/PurchasingSkeleton/PurchasingSkeleton";
import { usePurchaseOrders } from "../hooks/usePurchaseOrders";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function PurchasingPage() {
  const {
    data: purchaseOrders,
    isLoading,
    error,
    refetch,
  } = usePurchaseOrders();

  if (isLoading) {
    return <PurchasingSkeleton />;
  }

  if (error || !purchaseOrders) {
    return (
      <QueryErrorState
        title="Unable to load purchase orders"
        description="We couldn't retrieve the purchasing information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <PurchasingHeader />

      <div className="mt-8">
        {purchaseOrders.length === 0 ? (
          <PurchasingEmptyState />
        ) : (
          <PurchaseOrdersTable
            purchaseOrders={purchaseOrders}
          />
        )}
      </div>
    </>
  );
}