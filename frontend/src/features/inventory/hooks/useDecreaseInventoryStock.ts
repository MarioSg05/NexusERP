import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { decreaseInventoryStock } from "../services/inventoryService";

import type { InventoryStockRequest } from "../models/InventoryStockModel";

interface DecreaseInventoryStockVariables {
  id: string;
  request: InventoryStockRequest;
}

export function useDecreaseInventoryStock() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: DecreaseInventoryStockVariables) =>
      decreaseInventoryStock(id, request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["inventory"],
      });
    },
  });
}