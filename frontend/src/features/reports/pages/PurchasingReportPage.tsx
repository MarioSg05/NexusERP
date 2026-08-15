import { useState } from "react";

import { PurchasingReportTable } from "../components/PurchasingReportTable/PurchasingReportTable";
import { ReportDateFilter } from "../components/ReportDateFilter/ReportDateFilter";
import { ReportEmptyState } from "../components/ReportEmptyState/ReportEmptyState";
import { ReportTableSkeleton } from "../components/ReportTableSkeleton/ReportTableSkeleton";

import { usePurchasingReport } from "../hooks/usePurchasingReport";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import type { ReportDateFilters } from "../models/SalesReportModel";

export function PurchasingReportPage() {
  const [filters, setFilters] =
    useState<ReportDateFilters>({});

  const {
    data: items,
    isLoading,
    error,
    refetch,
  } = usePurchasingReport(filters);

  return (
    <div>
      <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Purchase Orders Report
        </h2>

        <p className="mt-2 text-sm text-slate-500">
          Purchase orders filtered by an optional date range.
        </p>
      </div>

      <div className="mt-6">
        <ReportDateFilter
          filters={filters}
          onApply={setFilters}
        />
      </div>

      <div className="mt-6">
        {isLoading ? (
          <ReportTableSkeleton />
        ) : error || !items ? (
          <QueryErrorState
            title="Unable to load purchasing report"
            description="We couldn't retrieve the purchase orders report."
            onRetry={() => {
              void refetch();
            }}
          />
        ) : items.length === 0 ? (
          <ReportEmptyState
            title="No purchase orders found"
            description="No purchase orders match the selected date range."
          />
        ) : (
          <PurchasingReportTable
            items={items}
          />
        )}
      </div>
    </div>
  );
}