import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { createSalesOrder } from "../services/salesService";

export function useCreateSalesOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createSalesOrder,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["sales-orders"],
      });
    },
  });
}