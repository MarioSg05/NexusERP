export interface CreatePurchaseOrderItemRequest {
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface CreatePurchaseOrderRequest {
  supplierId: string;
  items: CreatePurchaseOrderItemRequest[];
}

export interface CreatePurchaseOrderResponse {
  id: string;
  supplierId: string;
}