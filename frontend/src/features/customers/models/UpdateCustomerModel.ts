import type { CustomerType } from "./RegisterCustomerModel";

export interface UpdateCustomerRequest {
  name: string;
  email: string;
  phone: string | null;
  type: CustomerType;
}

export interface UpdateCustomerResponse {
  id: string;
  email: string;
}