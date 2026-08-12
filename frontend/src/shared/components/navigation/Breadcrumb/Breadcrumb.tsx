import { Link } from "react-router-dom";

import type { BreadcrumbItem } from "./types";

interface BreadcrumbProps {
  items: BreadcrumbItem[];
}

export function Breadcrumb({
  items,
}: BreadcrumbProps) {
  return (
    <nav
      aria-label="Breadcrumb"
      className="flex items-center gap-2 text-sm text-slate-500"
    >
      {items.map((item, index) => (
        <div
          key={`${item.label}-${index}`}
          className="flex items-center gap-2"
        >
          {index > 0 && (
            <span
              aria-hidden="true"
              className="text-slate-300"
            >
              /
            </span>
          )}

          {item.href ? (
            <Link
              to={item.href}
              className="transition-colors hover:text-slate-900"
            >
              {item.label}
            </Link>
          ) : (
            <span>{item.label}</span>
          )}
        </div>
      ))}
    </nav>
  );
}