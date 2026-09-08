import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { adjustInventoryStock } from "../services/inventoryService";

import type { InventoryStockRequest } from "../models/InventoryStockModel";

interface AdjustInventoryStockVariables {
  id: string;
  request: InventoryStockRequest;
}

export function useAdjustInventoryStock() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: AdjustInventoryStockVariables) =>
      adjustInventoryStock(id, request),

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["inventory"],
      });
    },
  });
}