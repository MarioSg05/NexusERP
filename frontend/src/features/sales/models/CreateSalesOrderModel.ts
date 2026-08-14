export interface CreateSalesOrderItemRequest {
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface CreateSalesOrderRequest {
  customerId: string;
  items: CreateSalesOrderItemRequest[];
}

export interface CreateSalesOrderResponse {
  id: string;
  customerId: string;
}