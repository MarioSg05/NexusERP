import type { SalesOrderStatus } from "./SalesOrderModel";

export interface SalesOrderItemDetail {
  id: string;
  productId: string;
  productName: string;
  sku: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface SalesOrderDetail {
  id: string;
  customerId: string;
  customerName: string;
  orderDate: string;
  status: SalesOrderStatus;
  total: number;
  items: SalesOrderItemDetail[];
}