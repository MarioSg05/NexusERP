import { Pencil } from "lucide-react";
import { Link } from "react-router-dom";

import type { User } from "../../models/UserModel";

interface UsersTableProps {
  users: User[];
}

function getRoleClasses(
  role: User["role"],
): string {
  switch (role) {
    case "Administrator":
      return "bg-purple-50 text-purple-700";

    case "Manager":
      return "bg-blue-50 text-blue-700";

    case "Viewer":
      return "bg-slate-100 text-slate-700";
  }
}

export function UsersTable({
  users,
}: UsersTableProps) {
  return (
    <div className="overflow-hidden rounded-xl border border-slate-200">
      <div className="overflow-x-auto">
        <table className="w-full border-collapse text-left">
          <thead className="bg-slate-50">
            <tr className="border-b border-slate-200">
              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                User
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Email
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Role
              </th>

              <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                Status
              </th>

              <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                Actions
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-200 bg-white">
            {users.map((user) => (
              <tr
                key={user.id}
                className="transition-colors hover:bg-slate-50"
              >
                <td className="px-6 py-4">
                  <span className="font-medium text-slate-900">
                    {user.firstName}{" "}
                    {user.lastName}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm text-slate-600">
                  {user.email}
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      getRoleClasses(user.role),
                    ].join(" ")}
                  >
                    {user.role}
                  </span>
                </td>

                <td className="px-6 py-4">
                  <span
                    className={[
                      "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                      user.isActive
                        ? "bg-emerald-50 text-emerald-700"
                        : "bg-red-50 text-red-700",
                    ].join(" ")}
                  >
                    {user.isActive
                      ? "Active"
                      : "Inactive"}
                  </span>
                </td>

                <td className="px-6 py-4 text-right">
                  <Link
                    to={`/users/${user.id}/edit`}
                    aria-label={`Edit ${user.firstName} ${user.lastName}`}
                    title={`Edit ${user.firstName} ${user.lastName}`}
                    className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900"
                  >
                    <Pencil size={16} />
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