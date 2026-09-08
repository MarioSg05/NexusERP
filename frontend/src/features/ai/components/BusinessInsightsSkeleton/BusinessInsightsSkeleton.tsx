export function BusinessInsightsSkeleton() {
  return (
    <div
      className="animate-pulse"
      aria-label="Generating business insights"
      aria-busy="true"
    >
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
        {Array.from({ length: 3 }).map((_, index) => (
          <div
            key={index}
            className="h-52 rounded-xl border border-slate-200 bg-slate-100"
          />
        ))}
      </div>

      <div className="mt-6 h-40 rounded-xl border border-slate-200 bg-slate-100" />

      <div className="mt-6 h-48 rounded-xl border border-slate-200 bg-slate-100" />
    </div>
  );
}