import { apiClient } from "../../../shared/api/client";

import type { Product } from "../models/ProductModel";

import type {
  RegisterProductRequest,
  RegisterProductResponse,
} from "../models/RegisterProductModel";

import type {
  UpdateProductRequest,
  UpdateProductResponse,
} from "../models/UpdateProductModel";

export async function getProducts(): Promise<Product[]> {
  const response =
    await apiClient.get<Product[]>("/products");

  return response.data;
}

export async function getProductById(
  id: string,
): Promise<Product> {
  const response =
    await apiClient.get<Product>(
      `/products/${id}`,
    );

  return response.data;
}

export async function registerProduct(
  request: RegisterProductRequest,
): Promise<RegisterProductResponse> {
  const response =
    await apiClient.post<RegisterProductResponse>(
      "/products",
      request,
    );

  return response.data;
}

export async function updateProduct(
  id: string,
  request: UpdateProductRequest,
): Promise<UpdateProductResponse> {
  const response =
    await apiClient.put<UpdateProductResponse>(
      `/products/${id}`,
      request,
    );

  return response.data;
}