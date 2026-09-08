import { Users } from "lucide-react";

export function CustomersEmptyState() {
  return (
    <div className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-dashed border-slate-300 bg-slate-50 p-8 text-center">
      <div className="flex h-12 w-12 items-center justify-center rounded-full bg-slate-100 text-slate-600">
        <Users size={24} />
      </div>

      <h2 className="mt-4 text-lg font-semibold text-slate-900">
        No customers yet
      </h2>

      <p className="mt-2 max-w-md text-sm text-slate-500">
        Customers registered in NexusERP will appear here.
      </p>
    </div>
  );
}