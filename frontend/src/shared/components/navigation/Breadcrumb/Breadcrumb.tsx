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
          key={item.label}
          className="flex items-center gap-2"
        >
          {index > 0 && (
            <span>/</span>
          )}

          <span>{item.label}</span>
        </div>
      ))}
    </nav>
  );
}