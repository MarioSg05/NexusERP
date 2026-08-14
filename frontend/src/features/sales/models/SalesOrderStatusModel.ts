import type { SalesOrderStatus } from "./SalesOrderModel";

export interface SalesOrderStatusResponse {
  id: string;
  status: SalesOrderStatus;
}