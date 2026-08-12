import { apiClient } from "../../../shared/api/client";

import type { Customer } from "../models/CustomerModel";

import type {
  RegisterCustomerRequest,
  RegisterCustomerResponse,
} from "../models/RegisterCustomerModel";

import type {
  UpdateCustomerRequest,
  UpdateCustomerResponse,
} from "../models/UpdateCustomerModel";

export async function getCustomers(): Promise<Customer[]> {
  const response =
    await apiClient.get<Customer[]>("/customers");

  return response.data;
}

export async function getCustomerById(
  id: string,
): Promise<Customer> {
  const response =
    await apiClient.get<Customer>(
      `/customers/${id}`,
    );

  return response.data;
}

export async function registerCustomer(
  request: RegisterCustomerRequest,
): Promise<RegisterCustomerResponse> {
  const response =
    await apiClient.post<RegisterCustomerResponse>(
      "/customers",
      request,
    );

  return response.data;
}

export async function updateCustomer(
  id: string,
  request: UpdateCustomerRequest,
): Promise<UpdateCustomerResponse> {
  const response =
    await apiClient.put<UpdateCustomerResponse>(
      `/customers/${id}`,
      request,
    );

  return response.data;
}