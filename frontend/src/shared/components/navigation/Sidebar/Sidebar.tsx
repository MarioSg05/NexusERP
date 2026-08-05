import { NavLink } from "react-router-dom";

import { navigationItems } from "./navigation";

export function Sidebar() {
  return (
    <aside className="flex h-[calc(100vh-64px)] w-64 flex-col border-r border-slate-800 bg-slate-900">
      <nav className="flex flex-1 flex-col py-4">
        {navigationItems.map((item) => {
          const Icon = item.icon;

          return (
            <NavLink
              key={item.path}
              to={item.path}
              className={({ isActive }) =>
                [
                  "mx-3 flex items-center gap-3 rounded-lg px-4 py-3 text-sm font-medium transition-colors",
                  isActive
                    ? "bg-blue-600 text-white"
                    : "text-slate-300 hover:bg-slate-800 hover:text-white",
                ].join(" ")
              }
            >
              <Icon size={18} />

              <span>{item.label}</span>
            </NavLink>
          );
        })}
      </nav>
    </aside>
  );
}