import { NavLink, Outlet } from "react-router-dom";

const reportTabs = [
  {
    label: "Inventory",
    to: "/reports/inventory",
  },
  {
    label: "Low Stock",
    to: "/reports/low-stock",
  },
  {
    label: "Sales",
    to: "/reports/sales",
  },
  {
    label: "Purchasing",
    to: "/reports/purchasing",
  },
];

export function ReportsLayout() {
  return (
    <div>
      <header>
        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          Reports
        </h1>

        <p className="mt-2 text-slate-500">
          Operational reports across NexusERP.
        </p>
      </header>

      <nav
        aria-label="Report navigation"
        className="mt-8 border-b border-slate-200"
      >
        <div className="flex gap-6 overflow-x-auto">
          {reportTabs.map((tab) => (
            <NavLink
              key={tab.to}
              to={tab.to}
              className={({ isActive }) =>
                [
                  "whitespace-nowrap border-b-2 px-1 pb-3 text-sm font-medium transition-colors",
                  isActive
                    ? "border-blue-600 text-blue-600"
                    : "border-transparent text-slate-500 hover:border-slate-300 hover:text-slate-900",
                ].join(" ")
              }
            >
              {tab.label}
            </NavLink>
          ))}
        </div>
      </nav>

      <div className="mt-8">
        <Outlet />
      </div>
    </div>
  );
}