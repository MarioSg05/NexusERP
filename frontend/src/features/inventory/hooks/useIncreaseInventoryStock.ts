import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { increaseInventoryStock } from "../services/inventoryService";

import type { InventoryStockRequest } from "../models/InventoryStockModel";

interface IncreaseInventoryStockVariables {
  id: string;
  request: InventoryStockRequest;
}

export function useIncreaseInventoryStock() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: IncreaseInventoryStockVariables) =>
      increaseInventoryStock(id, request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["inventory"],
      });
    },
  });
} 