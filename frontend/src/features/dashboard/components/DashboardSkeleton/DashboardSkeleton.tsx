export function DashboardSkeleton() {
  return (
    <div
      className="animate-pulse"
      aria-label="Loading dashboard"
      aria-busy="true"
    >
      <div className="mb-8">
        <div className="h-8 w-48 rounded bg-slate-200" />
        <div className="mt-3 h-4 w-72 rounded bg-slate-200" />
      </div>

      <div className="grid grid-cols-3 gap-6">
        {Array.from({ length: 3 }).map((_, index) => (
          <div
            key={index}
            className="h-32 rounded-xl border border-slate-200 bg-slate-100"
          />
        ))}
      </div>

      <div className="mt-8 space-y-6">
        {Array.from({ length: 3 }).map((_, index) => (
          <div
            key={index}
            className="h-44 rounded-xl border border-slate-200 bg-slate-100"
          />
        ))}
      </div>
    </div>
  );
}