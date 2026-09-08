import { useQuery } from "@tanstack/react-query";

import { getLowStockReport } from "../services/reportService";

export function useLowStockReport(
  minimumStock: number,
) {
  return useQuery({
    queryKey: [
      "reports",
      "low-stock",
      minimumStock,
    ],
    queryFn: () =>
      getLowStockReport(minimumStock),
  });
}