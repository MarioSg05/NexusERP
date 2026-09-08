import { Building2 } from "lucide-react";

export function SuppliersEmptyState() {
  return (
    <div className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-dashed border-slate-300 bg-slate-50 p-8 text-center">
      <div className="flex h-12 w-12 items-center justify-center rounded-full bg-slate-100 text-slate-600">
        <Building2 size={24} />
      </div>

      <h2 className="mt-4 text-lg font-semibold text-slate-900">
        No suppliers yet
      </h2>

      <p className="mt-2 max-w-md text-sm text-slate-500">
        Suppliers registered in NexusERP will appear here.
      </p>
    </div>
  );
}