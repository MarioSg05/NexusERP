import { Pencil } from "lucide-react";
import { Link } from "react-router-dom";

import { useAuth } from "../../../auth/hooks/useAuth";

import type { Supplier } from "../../models/SupplierModel";

interface SuppliersTableProps {
  suppliers: Supplier[];
}

export function SuppliersTable({
  suppliers,
}: SuppliersTableProps) {
  const { canManageErp } = useAuth();

  return (
    <div className="overflow-hidden rounded-xl border border-slate-200">
      <div className="overflow-x-auto">
        <table className="w-full border-collapse text-left">
          <thead className="bg-slate-50">
            <tr className="border-b border-slate-200">
              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Supplier
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Tax Identifier
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </th>

              {canManageErp && (
                <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                  Actions
                </th>
              )}
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-200 bg-white">
            {suppliers.map((supplier) => (
              <tr
                key={supplier.id}
                className="transition-colors hover:bg-slate-50"
              >
                <td className="px-6 py-4">
                  <span className="font-medium text-slate-900">
                    {supplier.name}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm text-slate-600">
                  {supplier.taxIdentifier}
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      supplier.isActive
                        ? "bg-emerald-50 text-emerald-700"
                        : "bg-red-50 text-red-700",
                    ].join(" ")}
                  >
                    {supplier.isActive
                      ? "Active"
                      : "Inactive"}
                  </span>
                </td>

                {canManageErp && (
                  <td className="px-6 py-4 text-right">
                    <Link
                      to={`/suppliers/${supplier.id}/edit`}
                      aria-label={`Edit ${supplier.name}`}
                      title={`Edit ${supplier.name}`}
                      className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900"
                    >
                      <Pencil size={16} />
                    </Link>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}