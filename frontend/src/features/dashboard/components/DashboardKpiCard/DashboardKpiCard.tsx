interface DashboardKpiCardProps {
  title: string;
  value: number;
}

export function DashboardKpiCard({
  title,
  value,
}: DashboardKpiCardProps) {
  return (
    <article className="rounded-xl border border-slate-200 bg-white p-6 shadow-sm">
      <h2 className="text-sm font-medium text-slate-500">
        {title}
      </h2>

      <p className="mt-3 text-4xl font-bold text-slate-900">
        {value}
      </p>
    </article>
  );
}