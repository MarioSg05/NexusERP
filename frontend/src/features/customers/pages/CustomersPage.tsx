import { CustomersEmptyState } from "../components/CustomersEmptyState/CustomersEmptyState";
import { CustomersHeader } from "../components/CustomersHeader/CustomersHeader";
import { CustomersSkeleton } from "../components/CustomersSkeleton/CustomersSkeleton";
import { CustomersTable } from "../components/CustomersTable/CustomersTable";
import { useCustomers } from "../hooks/useCustomers";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function CustomersPage() {
  const {
    data: customers,
    isLoading,
    error,
    refetch,
  } = useCustomers();

  if (isLoading) {
    return <CustomersSkeleton />;
  }

  if (error || !customers) {
    return (
      <QueryErrorState
        title="Unable to load customers"
        description="We couldn't retrieve the customer information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <CustomersHeader />

      <div className="mt-8">
        {customers.length === 0 ? (
          <CustomersEmptyState />
        ) : (
          <CustomersTable customers={customers} />
        )}
      </div>
    </>
  );
}