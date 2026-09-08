import { useQuery } from "@tanstack/react-query";

import { getInventory } from "../services/inventoryService";

export function useInventory() {
  return useQuery({
    queryKey: ["inventory"],
    queryFn: getInventory,
  });
}