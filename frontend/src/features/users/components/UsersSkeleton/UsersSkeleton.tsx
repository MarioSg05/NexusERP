export function UsersSkeleton() {
  return (
    <div className="animate-pulse">
      <div className="h-9 w-40 rounded bg-slate-200" />

      <div className="mt-3 h-5 w-80 rounded bg-slate-200" />

      <div className="mt-8 overflow-hidden rounded-xl border border-slate-200">
        <div className="h-14 bg-slate-100" />

        {Array.from({ length: 5 }).map(
          (_, index) => (
            <div
              key={index}
              className="h-16 border-t border-slate-200 bg-white"
            />
          ),
        )}
      </div>
    </div>
  );
}