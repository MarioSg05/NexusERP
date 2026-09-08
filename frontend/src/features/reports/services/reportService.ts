import { apiClient } from "../../../shared/api/client";

import type { InventoryReportItem } from "../models/InventoryReportModel";
import type {
  ReportDateFilters,
  SalesReportItem,
} from "../models/SalesReportModel";
import type { PurchasingReportItem } from "../models/PurchasingReportModel";

export async function getInventoryReport(): Promise<
  InventoryReportItem[]
> {
  const response =
    await apiClient.get<InventoryReportItem[]>(
      "/reports/inventory",
    );

  return response.data;
}

export async function getLowStockReport(
  minimumStock: number,
): Promise<InventoryReportItem[]> {
  const response =
    await apiClient.get<InventoryReportItem[]>(
      "/reports/low-stock",
      {
        params: {
          minimumStock,
        },
      },
    );

  return response.data;
}

export async function getSalesReport(
  filters: ReportDateFilters,
): Promise<SalesReportItem[]> {
  const response =
    await apiClient.get<SalesReportItem[]>(
      "/reports/sales",
      {
        params: {
          from: filters.from || undefined,
          to: filters.to || undefined,
        },
      },
    );

  return response.data;
}

export async function getPurchasingReport(
  filters: ReportDateFilters,
): Promise<PurchasingReportItem[]> {
  const response =
    await apiClient.get<PurchasingReportItem[]>(
      "/reports/purchasing",
      {
        params: {
          from: filters.from || undefined,
          to: filters.to || undefined,
        },
      },
    );

  return response.data;
}