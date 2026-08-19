import { UsersEmptyState } from "../components/UsersEmptyState/UsersEmptyState";
import { UsersHeader } from "../components/UsersHeader/UsersHeader";
import { UsersSkeleton } from "../components/UsersSkeleton/UsersSkeleton";
import { UsersTable } from "../components/UsersTable/UsersTable";

import { useUsers } from "../hooks/useUsers";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function UsersPage() {
  const {
    data: users,
    isLoading,
    error,
    refetch,
  } = useUsers();

  if (isLoading) {
    return <UsersSkeleton />;
  }

  if (error || !users) {
    return (
      <QueryErrorState
        title="Unable to load users"
        description="We couldn't retrieve the user information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <UsersHeader />

      <div className="mt-8">
        {users.length === 0 ? (
          <UsersEmptyState />
        ) : (
          <UsersTable users={users} />
        )}
      </div>
    </>
  );
}