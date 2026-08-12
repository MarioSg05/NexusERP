import type { BreadcrumbItem } from "./types";

const breadcrumbMap: Record<string, BreadcrumbItem[]> = {
    "/": [
        {
            label: "Dashboard",
        },
    ],

    "/customers": [
        {
            label: "Customers",
        },
    ],
    "/customers/new": [
        {
            label: "Customers",
            href: "/customers",
        },
        {
            label: "New Customer",
        },
    ],
};

export function getBreadcrumbItems(pathname: string): BreadcrumbItem[] {
    if (pathname.startsWith("/customers/") && pathname.endsWith("/edit")) {
        return [
            {
                label: "Customers",
                href: "/customers",
            },
            {
                label: "Edit Customer",
            },
        ];
    }
    return breadcrumbMap[pathname] ?? [];
}
