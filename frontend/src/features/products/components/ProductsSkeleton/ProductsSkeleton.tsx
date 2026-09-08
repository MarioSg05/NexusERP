export function ProductsSkeleton() {
  return (
    <div
      className="animate-pulse"
      aria-label="Loading products"
      aria-busy="true"
    >
      <div>
        <div className="h-8 w-44 rounded bg-slate-200" />
        <div className="mt-3 h-4 w-80 rounded bg-slate-200" />
      </div>

      <div className="mt-8 overflow-hidden rounded-xl border border-slate-200">
        <div className="h-14 border-b border-slate-200 bg-slate-100" />

        {Array.from({ length: 5 }).map((_, index) => (
          <div
            key={index}
            className="h-16 border-b border-slate-200 bg-white last:border-b-0"
          />
        ))}
      </div>
    </div>
  );
}