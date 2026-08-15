import type { InventoryReportItem } from "../../models/InventoryReportModel";

interface InventoryReportTableProps {
  items: InventoryReportItem[];
}

export function InventoryReportTable({
  items,
}: InventoryReportTableProps) {
  return (
    <div className="overflow-hidden rounded-xl border border-slate-200">
      <div className="overflow-x-auto">
        <table className="w-full border-collapse text-left">
          <thead className="bg-slate-50">
            <tr className="border-b border-slate-200">
              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Product
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                SKU
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Quantity
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-200 bg-white">
            {items.map((item) => (
              <tr
                key={item.productId}
                className="transition-colors hover:bg-slate-50"
              >
                <td className="px-6 py-4">
                  <span className="font-medium text-slate-900">
                    {item.productName}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm text-slate-600">
                  {item.sku}
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "text-sm font-semibold",
                      item.quantity === 0
                        ? "text-red-600"
                        : "text-slate-900",
                    ].join(" ")}
                  >
                    {item.quantity}
                  </span>
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      item.isActive
                        ? "bg-emerald-50 text-emerald-700"
                        : "bg-slate-100 text-slate-600",
                    ].join(" ")}
                  >
                    {item.isActive
                      ? "Active"
                      : "Inactive"}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}