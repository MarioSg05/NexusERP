import { apiClient } from "../../../shared/api/client";

import type { Dashboard } from "../models/DashboardModel";

export async function getDashboard() {
  const response =
    await apiClient.get<Dashboard>("/dashboard");

  return response.data;
}