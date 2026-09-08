import type { LucideIcon } from "lucide-react";

interface BusinessInsightsSectionProps {
  title: string;
  items: string[];
  icon: LucideIcon;
}

export function BusinessInsightsSection({
  title,
  items,
  icon: Icon,
}: BusinessInsightsSectionProps) {
  return (
    <section className="rounded-xl border border-slate-200 bg-white p-6">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-slate-100 text-slate-700">
          <Icon size={20} />
        </div>

        <h2 className="text-lg font-semibold text-slate-900">
          {title}
        </h2>
      </div>

      <ul className="mt-5 space-y-3">
        {items.map((item) => (
          <li
            key={item}
            className="flex gap-3 text-sm leading-6 text-slate-600"
          >
            <span
              className="mt-2 h-1.5 w-1.5 shrink-0 rounded-full bg-slate-400"
              aria-hidden="true"
            />

            <span>{item}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}