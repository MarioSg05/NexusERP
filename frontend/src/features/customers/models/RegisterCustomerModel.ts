export type CustomerType = 1 | 2;

export interface RegisterCustomerRequest {
  name: string;
  email: string;
  phone: string | null;
  type: CustomerType;
}

export interface RegisterCustomerResponse {
  id: string;
  email: string;
}