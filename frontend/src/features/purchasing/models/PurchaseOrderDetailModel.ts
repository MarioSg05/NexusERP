import type { PurchaseOrderStatus } from "./PurchaseOrderModel";

export interface PurchaseOrderItemDetail {
  id: string;
  productId: string;
  productName: string;
  sku: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface PurchaseOrderDetail {
  id: string;
  supplierId: string;
  supplierName: string;
  orderDate: string;
  status: PurchaseOrderStatus;
  total: number;
  items: PurchaseOrderItemDetail[];
}