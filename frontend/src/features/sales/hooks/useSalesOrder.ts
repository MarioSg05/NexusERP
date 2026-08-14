import { useQuery } from "@tanstack/react-query";

import { getSalesOrderById } from "../services/salesService";

export function useSalesOrder(
  id: string,
) {
  return useQuery({
    queryKey: ["sales-orders", id],
    queryFn: () =>
      getSalesOrderById(id),
    enabled: Boolean(id),
  });
}