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
    "/products": [
        {
            label: "Products",
        },
    ],
    "/products/new": [
        {
            label: "Products",
            href: "/products",
        },
        {
            label: "New Product",
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

    if (pathname.startsWith("/products/") && pathname.endsWith("/edit")) {
        return [
            {
                label: "Products",
                href: "/products",
            },
            {
                label: "Edit Product",
            },
        ];
    }
    return breadcrumbMap[pathname] ?? [];
}
