import { InventoryReportTable } from "../components/InventoryReportTable/InventoryReportTable";
import { ReportEmptyState } from "../components/ReportEmptyState/ReportEmptyState";
import { ReportTableSkeleton } from "../components/ReportTableSkeleton/ReportTableSkeleton";

import { useInventoryReport } from "../hooks/useInventoryReport";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function InventoryReportPage() {
  const {
    data: items,
    isLoading,
    error,
    refetch,
  } = useInventoryReport();

  return (
    <div>
      <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Inventory Report
        </h2>

        <p className="mt-2 text-sm text-slate-500">
          Current inventory quantities by product.
        </p>
      </div>

      <div className="mt-6">
        {isLoading ? (
          <ReportTableSkeleton />
        ) : error || !items ? (
          <QueryErrorState
            title="Unable to load inventory report"
            description="We couldn't retrieve the inventory report."
            onRetry={() => {
              void refetch();
            }}
          />
        ) : items.length === 0 ? (
          <ReportEmptyState
            title="No inventory data"
            description="There are no inventory records available for this report."
          />
        ) : (
          <InventoryReportTable
            items={items}
          />
        )}
      </div>
    </div>
  );
}