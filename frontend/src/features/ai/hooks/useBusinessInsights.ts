import { useQuery } from "@tanstack/react-query";

import type { BusinessInsights } from "../models/BusinessInsightsModel";

export const businessInsightsQueryKey = [
  "business-insights",
] as const;

export function useBusinessInsights() {
  return useQuery<BusinessInsights>({
    queryKey: businessInsightsQueryKey,

    queryFn: async () => {
      throw new Error(
        "Business insights must be generated explicitly.",
      );
    },

    enabled: false,

    staleTime: Infinity,
  });
}