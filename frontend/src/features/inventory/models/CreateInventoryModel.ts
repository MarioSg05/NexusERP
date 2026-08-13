export interface CreateInventoryRequest {
  productId: string;
  quantity: number;
}

export interface CreateInventoryResponse {
  id: string;
  productId: string;
  quantity: number;
}