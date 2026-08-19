import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { deactivateUser } from "../services/userService";

export function useDeactivateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deactivateUser,

    onSuccess: async (_, id) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["users"],
        }),

        queryClient.invalidateQueries({
          queryKey: ["users", id],
        }),
      ]);
    },
  });
}