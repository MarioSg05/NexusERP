import type { PurchaseOrderStatus } from "./PurchaseOrderModel";

export interface PurchaseOrderStatusResponse {
  id: string;
  status: PurchaseOrderStatus;
}