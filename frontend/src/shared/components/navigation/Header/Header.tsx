import {
  LogOut,
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

      <nav className="flex items-center">
        {user && (
          <div className="flex items-center gap-3">
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