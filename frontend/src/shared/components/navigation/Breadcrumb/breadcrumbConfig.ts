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
    "/inventory": [
        {
            label: "Inventory",
        },
    ],
    "/inventory/new": [
        {
            label: "Inventory",
            href: "/inventory",
        },
        {
            label: "Create Inventory",
        },
    ],
    "/purchasing": [
        {
            label: "Purchasing",
        },
    ],
    "/purchasing/new": [
        {
            label: "Purchasing",
            href: "/purchasing",
        },
        {
            label: "New Purchase Order",
        },
    ],
    "/sales": [
        {
            label: "Sales",
        },
    ],
    "/sales/new": [
        {
            label: "Sales",
            href: "/sales",
        },
        {
            label: "New Sales Order",
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

    if (pathname.startsWith("/purchasing/") && pathname !== "/purchasing/new") {
        return [
            {
                label: "Purchasing",
                href: "/purchasing",
            },
            {
                label: "Purchase Order",
            },
        ];
    }

    if (pathname.startsWith("/sales/") && pathname !== "/sales/new") {
        return [
            {
                label: "Sales",
                href: "/sales",
            },
            {
                label: "Sales Order",
            },
        ];
    }
    return breadcrumbMap[pathname] ?? [];
}
