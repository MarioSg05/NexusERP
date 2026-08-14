export type PurchaseOrderStatus =
  | "Pending"
  | "Approved"
  | "Cancelled";

export interface PurchaseOrder {
  id: string;
  supplierId: string;
  supplierName: string;
  orderDate: string;
  status: PurchaseOrderStatus;
  total: number;
}