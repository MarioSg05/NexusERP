export function ReportTableSkeleton() {
  return (
    <div
      className="animate-pulse overflow-hidden rounded-xl border border-slate-200"
      aria-label="Loading report"
      aria-busy="true"
    >
      <div className="h-14 border-b border-slate-200 bg-slate-100" />

      {Array.from({ length: 5 }).map(
        (_, index) => (
          <div
            key={index}
            className="h-16 border-b border-slate-200 bg-white last:border-b-0"
          />
        ),
      )}
    </div>
  );
}