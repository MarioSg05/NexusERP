import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { activateUser } from "../services/userService";

export function useActivateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: activateUser,

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