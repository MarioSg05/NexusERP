import {
  Bell,
  CircleHelp,
  LogOut,
  Search,
  UserRound,
} from "lucide-react";

import { useAuth } from "../../../../features/auth/hooks/useAuth";

export function Header() {
  const {
    user,
    logout,
  } = useAuth();

  const fullName =
    user
      ? `${user.firstName} ${user.lastName}`
      : "";

  return (
    <header className="flex h-16 items-center justify-between border-b border-slate-200 bg-white px-8 shadow-sm">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          NexusERP
        </h1>
      </div>

      <div className="mx-8 flex max-w-xl flex-1 items-center rounded-lg border border-slate-300 px-3 py-2">
        <Search
          size={18}
          className="mr-2 text-slate-500"
        />

        <input
          type="search"
          placeholder="Search..."
          className="w-full border-none bg-transparent outline-none"
        />
      </div>

      <nav className="flex items-center gap-4">
        <button
          type="button"
          aria-label="Notifications"
          className="rounded-lg p-2 text-slate-600 transition-colors hover:bg-slate-100 hover:text-slate-900"
        >
          <Bell size={18} />
        </button>

        <button
          type="button"
          aria-label="Help"
          className="rounded-lg p-2 text-slate-600 transition-colors hover:bg-slate-100 hover:text-slate-900"
        >
          <CircleHelp size={18} />
        </button>

        {user && (
          <div className="flex items-center gap-3 border-l border-slate-200 pl-4">
            <div className="flex h-9 w-9 items-center justify-center rounded-full bg-blue-50 text-blue-600">
              <UserRound size={18} />
            </div>

            <div className="hidden min-w-0 lg:block">
              <p className="max-w-40 truncate text-sm font-medium text-slate-900">
                {fullName}
              </p>

              <p className="max-w-40 truncate text-xs text-slate-500">
                {user.email}
              </p>
            </div>

            <button
              type="button"
              onClick={logout}
              aria-label="Sign out"
              title="Sign out"
              className="rounded-lg p-2 text-slate-500 transition-colors hover:bg-red-50 hover:text-red-600"
            >
              <LogOut size={18} />
            </button>
          </div>
        )}
      </nav>
    </header>
  );
}