import { formatCurrency } from "../../../../shared/lib/formatCurrency";
import { formatDate } from "../../../../shared/lib/formatDate";

import type {
  SalesReportItem,
  SalesReportStatus,
} from "../../models/SalesReportModel";

interface SalesReportTableProps {
  items: SalesReportItem[];
}

function getStatusClasses(
  status: SalesReportStatus,
): string {
  switch (status) {
    case "Pending":
      return "bg-amber-50 text-amber-700";

    case "Confirmed":
      return "bg-emerald-50 text-emerald-700";

    case "Cancelled":
      return "bg-red-50 text-red-700";
  }
}

export function SalesReportTable({
  items,
}: SalesReportTableProps) {
  return (
    <div className="overflow-hidden rounded-xl border border-slate-200">
      <div className="overflow-x-auto">
        <table className="w-full border-collapse text-left">
          <thead className="bg-slate-50">
            <tr className="border-b border-slate-200">
              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Reference
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Customer
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Date
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </th>

              <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                Total
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-200 bg-white">
            {items.map((item) => (
              <tr
                key={item.salesOrderId}
                className="transition-colors hover:bg-slate-50"
              >
                <td className="px-6 py-4">
                  <span
                    title={item.salesOrderId}
                    className="font-mono text-sm font-medium text-slate-700"
                  >
                    {item.salesOrderId
                      .slice(0, 8)
                      .toUpperCase()}
                  </span>
                </td>

                <td className="px-6 py-4">
                  <span className="font-medium text-slate-900">
                    {item.customerName}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm text-slate-600">
                  {formatDate(
                    item.orderDate,
                  )}
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      getStatusClasses(
                        item.status,
                      ),
                    ].join(" ")}
                  >
                    {item.status}
                  </span>
                </td>

                <td className="px-6 py-4 text-right text-sm font-medium text-slate-900">
                  {formatCurrency(
                    item.total,
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}