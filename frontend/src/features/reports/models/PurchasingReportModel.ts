export type PurchasingReportStatus =
  | "Pending"
  | "Approved"
  | "Cancelled";

export interface PurchasingReportItem {
  purchaseOrderId: string;
  supplierId: string;
  supplierName: string;
  orderDate: string;
  status: PurchasingReportStatus;
  total: number;
}