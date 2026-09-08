import { useQuery } from "@tanstack/react-query";

import { getCustomerById } from "../services/customerService";

export function useCustomer(id: string) {
  return useQuery({
    queryKey: ["customers", id],
    queryFn: () => getCustomerById(id),
    enabled: Boolean(id),
  });
}