import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { cancelSalesOrder } from "../services/salesService";

export function useCancelSalesOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: cancelSalesOrder,

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
      ]);
    },
  });
}