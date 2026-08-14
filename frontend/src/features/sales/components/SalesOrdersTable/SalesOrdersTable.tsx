import { Eye } from "lucide-react";
import { Link } from "react-router-dom";

import { formatCurrency } from "../../../../shared/lib/formatCurrency";
import { formatDate } from "../../../../shared/lib/formatDate";

import type {
  SalesOrder,
  SalesOrderStatus,
} from "../../models/SalesOrderModel";

interface SalesOrdersTableProps {
  salesOrders: SalesOrder[];
}

function getStatusClasses(
  status: SalesOrderStatus,
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

export function SalesOrdersTable({
  salesOrders,
}: SalesOrdersTableProps) {
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

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Total
              </th>

              <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                Actions
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-200 bg-white">
            {salesOrders.map((salesOrder) => (
              <tr
                key={salesOrder.id}
                className="transition-colors hover:bg-slate-50"
              >
                <td className="px-6 py-4">
                  <span
                    className="font-mono text-sm font-medium text-slate-700"
                    title={salesOrder.id}
                  >
                    {salesOrder.id
                      .slice(0, 8)
                      .toUpperCase()}
                  </span>
                </td>

                <td className="px-6 py-4">
                  <span className="font-medium text-slate-900">
                    {salesOrder.customerName}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm text-slate-600">
                  {formatDate(
                    salesOrder.orderDate,
                  )}
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      getStatusClasses(
                        salesOrder.status,
                      ),
                    ].join(" ")}
                  >
                    {salesOrder.status}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm font-medium text-slate-900">
                  {formatCurrency(
                    salesOrder.total,
                  )}
                </td>

                <td className="px-6 py-4 text-right">
                  <Link
                    to={`/sales/${salesOrder.id}`}
                    aria-label={`View sales order ${salesOrder.id}`}
                    title="View sales order"
                    className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900"
                  >
                    <Eye size={16} />
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}