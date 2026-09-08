import { useState } from "react";

import { ReportDateFilter } from "../components/ReportDateFilter/ReportDateFilter";
import { ReportEmptyState } from "../components/ReportEmptyState/ReportEmptyState";
import { ReportTableSkeleton } from "../components/ReportTableSkeleton/ReportTableSkeleton";
import { SalesReportTable } from "../components/SalesReportTable/SalesReportTable";

import { useSalesReport } from "../hooks/useSalesReport";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import type { ReportDateFilters } from "../models/SalesReportModel";

export function SalesReportPage() {
  const [filters, setFilters] =
    useState<ReportDateFilters>({});

  const {
    data: items,
    isLoading,
    error,
    refetch,
  } = useSalesReport(filters);

  return (
    <div>
      <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Sales Orders Report
        </h2>

        <p className="mt-2 text-sm text-slate-500">
          Sales orders filtered by an optional date range.
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
            title="Unable to load sales report"
            description="We couldn't retrieve the sales orders report."
            onRetry={() => {
              void refetch();
            }}
          />
        ) : items.length === 0 ? (
          <ReportEmptyState
            title="No sales orders found"
            description="No sales orders match the selected date range."
          />
        ) : (
          <SalesReportTable
            items={items}
          />
        )}
      </div>
    </div>
  );
}