import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { changeUserRole } from "../services/userService";

import type { ChangeUserRoleRequest } from "../models/ChangeUserRoleModel";

interface ChangeUserRoleVariables {
  id: string;
  request: ChangeUserRoleRequest;
}

export function useChangeUserRole() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: ChangeUserRoleVariables) =>
      changeUserRole(id, request),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["users"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "users",
            variables.id,
          ],
        }),
      ]);
    },
  });
}