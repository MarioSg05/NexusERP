export interface Customer {
  id: string;
  name: string;
  email: string;
  phone: string | null;
  type: string;
  isActive: boolean;
}