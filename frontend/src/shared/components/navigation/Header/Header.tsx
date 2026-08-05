import { Bell, CircleHelp, Search } from "lucide-react";

export function Header() {
  return (
      <header className="flex h-16 items-center justify-between border-b border-slate-200 bg-white px-8 shadow-sm">      <div>
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

      <nav className="flex items-center gap-5">
        <button type="button">
          <Bell size={18} />
        </button>

        <button type="button">
          <CircleHelp size={18} />
        </button>

        <button
          type="button"
          className="font-medium"
        >
          Mario Rodríguez
        </button>
      </nav>
    </header>
  );
}