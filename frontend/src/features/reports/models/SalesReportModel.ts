export type SalesReportStatus =
  | "Pending"
  | "Confirmed"
  | "Cancelled";

export interface SalesReportItem {
  salesOrderId: string;
  customerId: string;
  customerName: string;
  orderDate: string;
  status: SalesReportStatus;
  total: number;
}

export interface ReportDateFilters {
  from?: string;
  to?: string;
}