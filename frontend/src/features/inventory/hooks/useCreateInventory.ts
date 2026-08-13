import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { createInventory } from "../services/inventoryService";

export function useCreateInventory() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createInventory,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["inventory"],
      });
    },
  });
}