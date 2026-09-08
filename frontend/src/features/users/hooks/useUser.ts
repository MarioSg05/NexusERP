import { useQuery } from "@tanstack/react-query";

import { getUserById } from "../services/userService";

export function useUser(id: string) {
  return useQuery({
    queryKey: ["users", id],
    queryFn: () => getUserById(id),
    enabled: Boolean(id),
  });
}