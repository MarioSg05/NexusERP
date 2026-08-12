export interface RegisterProductRequest {
  name: string;
  sku: string;
  price: number;
}

export interface RegisterProductResponse {
  id: string;
  sku: string;
}