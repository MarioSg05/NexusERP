import {
  Boxes,
  ChartColumn,
  LayoutDashboard,
  Package,
  ReceiptText,
  Settings,
  ShoppingCart,
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
    label: "Settings",
    path: "/settings",
    icon: Settings,
  },
];