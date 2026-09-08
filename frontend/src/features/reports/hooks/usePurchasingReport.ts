import { useQuery } from "@tanstack/react-query";

import type { ReportDateFilters } from "../models/SalesReportModel";
import { getPurchasingReport } from "../services/reportService";

export function usePurchasingReport(
  filters: ReportDateFilters,
) {
  return useQuery({
    queryKey: [
      "reports",
      "purchasing",
      filters.from ?? null,
      filters.to ?? null,
    ],
    queryFn: () =>
      getPurchasingReport(filters),
  });
}