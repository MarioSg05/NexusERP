import { useQuery } from "@tanstack/react-query";

import type { ReportDateFilters } from "../models/SalesReportModel";
import { getSalesReport } from "../services/reportService";

export function useSalesReport(
  filters: ReportDateFilters,
) {
  return useQuery({
    queryKey: [
      "reports",
      "sales",
      filters.from ?? null,
      filters.to ?? null,
    ],
    queryFn: () =>
      getSalesReport(filters),
  });
}