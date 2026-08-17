import {
  useCallback,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";

import { useQueryClient } from "@tanstack/react-query";

import {
  getCurrentUser,
  login as loginRequest,
} from "../services/authService";

import {
  clearAuthSession,
  hasValidAuthSession,
  saveAuthSession,
} from "../services/authSession";

import { subscribeToUnauthorized } from "../services/authEvents";

import type {
  CurrentUser,
  LoginRequest,
} from "../models/AuthModel";

import {
  AuthContext,
  type AuthContextValue,
} from "./AuthContext";

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({
  children,
}: AuthProviderProps) {
  const queryClient = useQueryClient();

  const [user, setUser] =
    useState<CurrentUser | null>(null);

  const [isLoading, setIsLoading] =
    useState(true);

  const clearAuthenticatedState =
    useCallback(() => {
      clearAuthSession();
      setUser(null);
      queryClient.clear();
    }, [queryClient]);

  useEffect(() => {
    let isMounted = true;

    async function restoreSession() {
      if (!hasValidAuthSession()) {
        if (isMounted) {
          setIsLoading(false);
        }

        return;
      }

      try {
        const currentUser =
          await getCurrentUser();

        if (isMounted) {
          setUser(currentUser);
        }
      } catch {
        clearAuthSession();

        if (isMounted) {
          setUser(null);
          queryClient.clear();
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    void restoreSession();

    return () => {
      isMounted = false;
    };
  }, [queryClient]);

  useEffect(() => {
    return subscribeToUnauthorized(
      clearAuthenticatedState,
    );
  }, [clearAuthenticatedState]);

  const login = useCallback(
    async (request: LoginRequest) => {
      const response =
        await loginRequest(request);

      saveAuthSession(
        response.accessToken,
        response.expiresAt,
      );

      try {
        const currentUser =
          await getCurrentUser();

        setUser(currentUser);
      } catch (error) {
        clearAuthenticatedState();

        throw error;
      }
    },
    [clearAuthenticatedState],
  );

  const logout = useCallback(() => {
    clearAuthenticatedState();
  }, [clearAuthenticatedState]);

  const value =
    useMemo<AuthContextValue>(
      () => ({
        user,
        isAuthenticated: user !== null,
        isLoading,
        login,
        logout,
      }),
      [
        user,
        isLoading,
        login,
        logout,
      ],
    );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}