import { useQuery } from "@tanstack/react-query";

import { getSalesOrders } from "../services/salesService";

export function useSalesOrders() {
  return useQuery({
    queryKey: ["sales-orders"],
    queryFn: getSalesOrders,
  });
}