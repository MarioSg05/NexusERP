import type { DashboardSummaryItem } from "./types";

interface DashboardSummaryCardProps {
  title: string;
  items: DashboardSummaryItem[];
}

export function DashboardSummaryCard({
  title,
  items,
}: DashboardSummaryCardProps) {
  return (
    <article className="rounded-xl border border-slate-200 bg-white p-6 shadow-sm">
      <h2 className="mb-6 text-lg font-semibold text-slate-900">
        {title}
      </h2>

      <div className="space-y-4">
        {items.map((item) => (
          <div
            key={item.label}
            className="flex items-center justify-between"
          >
            <span className="text-sm text-slate-500">
              {item.label}
            </span>

            <span className="text-lg font-semibold text-slate-900">
              {item.value}
            </span>
          </div>
        ))}
      </div>
    </article>
  );
}