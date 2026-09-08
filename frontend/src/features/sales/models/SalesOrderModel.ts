export type SalesOrderStatus =
  | "Pending"
  | "Confirmed"
  | "Cancelled";

export interface SalesOrder {
  id: string;
  customerId: string;
  customerName: string;
  orderDate: string;
  status: SalesOrderStatus;
  total: number;
}