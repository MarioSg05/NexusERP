import { SuppliersEmptyState } from "../components/SuppliersEmptyState/SuppliersEmptyState";
import { SuppliersHeader } from "../components/SuppliersHeader/SuppliersHeader";
import { SuppliersSkeleton } from "../components/SuppliersSkeleton/SuppliersSkeleton";
import { SuppliersTable } from "../components/SuppliersTable/SuppliersTable";

import { useSuppliers } from "../hooks/useSuppliers";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function SuppliersPage() {
  const {
    data: suppliers,
    isLoading,
    error,
    refetch,
  } = useSuppliers();

  if (isLoading) {
    return <SuppliersSkeleton />;
  }

  if (error || !suppliers) {
    return (
      <QueryErrorState
        title="Unable to load suppliers"
        description="We couldn't retrieve the supplier information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <SuppliersHeader />

      <div className="mt-8">
        {suppliers.length === 0 ? (
          <SuppliersEmptyState />
        ) : (
          <SuppliersTable
            suppliers={suppliers}
          />
        )}
      </div>
    </>
  );
}