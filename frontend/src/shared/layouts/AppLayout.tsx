import { Breadcrumb } from "../components/navigation/Breadcrumb/Breadcrumb";
import { Header } from "../components/navigation/Header/Header";
import { Sidebar } from "../components/navigation/Sidebar/Sidebar";
import { DashboardPage } from "../../features/dashboard/pages/DashboardPage";

export function AppLayout() {
  return (
    <div className="min-h-screen bg-slate-50">
      <Header />

      <div className="flex min-h-[calc(100vh-64px)]">
        <Sidebar />

        <main className="flex-1 p-8">
          <section className="mb-8">
            <Breadcrumb
              items={[
                {
                  label: "Dashboard",
                },
              ]}
            />
          </section>

          <section className="rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
            <DashboardPage />
          </section>
        </main>
      </div>
    </div>
  );
}