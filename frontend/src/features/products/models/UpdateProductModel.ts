export interface UpdateProductRequest {
  name: string;
  price: number;
}

export interface UpdateProductResponse {
  id: string;
  sku: string;
}