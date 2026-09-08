import {
  Outlet,
  useLocation,
} from "react-router-dom";

import { Breadcrumb } from "../components/navigation/Breadcrumb/Breadcrumb";
import { getBreadcrumbItems } from "../components/navigation/Breadcrumb/breadcrumbConfig";
import { Header } from "../components/navigation/Header/Header";
import { Sidebar } from "../components/navigation/Sidebar/Sidebar";

export function AppLayout() {
  const location = useLocation();

  const breadcrumbItems =
    getBreadcrumbItems(location.pathname);

  return (
    <div className="min-h-screen bg-slate-50">
      <Header />

      <div className="flex min-h-[calc(100vh-64px)]">
        <Sidebar />

        <main className="flex-1 p-8">
          {breadcrumbItems.length > 0 && (
            <section className="mb-8">
              <Breadcrumb items={breadcrumbItems} />
            </section>
          )}

          <section className="rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
            <Outlet />
          </section>
        </main>
      </div>
    </div>
  );
}