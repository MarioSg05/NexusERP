import {
  Boxes,
  BrainCircuit,
  Building2,
  ChartColumn,
  LayoutDashboard,
  Package,
  ReceiptText,
  ShoppingCart,
  UserCog,
  Users,
} from "lucide-react";

import type { NavigationItem } from "./types";

export const navigationItems: NavigationItem[] = [
  {
    label: "Dashboard",
    path: "/",
    icon: LayoutDashboard,
  },
  {
    label: "Customers",
    path: "/customers",
    icon: Users,
  },
  {
    label: "Products",
    path: "/products",
    icon: Package,
  },
  {
    label: "Inventory",
    path: "/inventory",
    icon: Boxes,
  },
  {
    label: "Suppliers",
    path: "/suppliers",
    icon: Building2,
  },
  {
    label: "Purchasing",
    path: "/purchasing",
    icon: ShoppingCart,
  },
  {
    label: "Sales",
    path: "/sales",
    icon: ReceiptText,
  },
  {
    label: "Reports",
    path: "/reports",
    icon: ChartColumn,
  },
  {
    label: "AI Insights",
    path: "/ai/business-insights",
    icon: BrainCircuit,
  },
  {
    label: "Users",
    path: "/users",
    icon: UserCog,
  },
];