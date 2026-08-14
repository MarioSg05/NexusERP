import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { confirmSalesOrder } from "../services/salesService";

export function useConfirmSalesOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: confirmSalesOrder,

    onSuccess: async (_, id) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["sales-orders"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "sales-orders",
            id,
          ],
        }),

        queryClient.invalidateQueries({
          queryKey: ["inventory"],
        }),
      ]);
    },
  });
}