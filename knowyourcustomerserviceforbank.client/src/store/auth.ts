import base from "@/utils/axios-base";
import { Store } from "@tanstack/react-store";
import { useSelector } from "@tanstack/react-store";
import { userResponse, type UserResponseType, type UserLoginType } from "@/validators/user";

export type AuthActions = {
  isSessionActive: () => Promise<boolean>;
  loginWithCred: (data: UserLoginType) => Promise<void>;
  logout: () => Promise<void>;
};

export interface AuthState {
  isAuthenticated: boolean;
  user: UserResponseType | null;
  error: string | null;
}

export const authStore = new Store<AuthState>({
  isAuthenticated: false,
  user: null,
  error: null,
});

export const authActions: AuthActions = {
  isSessionActive: async () => {
    return true;
    // try {
    //   const res = await base.get<UserResponseType>("/user/login", {
    //     schema: userResponse,
    //   });

    //   if (res.status === 200) {
    //     authStore.setState((state) => ({
    //       ...state,
    //       isAuthenticated: true,
    //       user: res.data,
    //       error: null,
    //     }));
    //     return true;
    //   } else {
    //     authStore.setState((state) => ({
    //       ...state,
    //       isAuthenticated: false,
    //       user: null,
    //       error: res.statusText,
    //     }));
    //   }
    // } catch (err) {
    //   authStore.setState((state) => ({
    //     ...state,
    //     isAuthenticated: false,
    //     user: null,
    //     error: "network_error",
    //   }));
    // }
    // return false;
  },

  loginWithCred: async (data: UserLoginType) => {
    const res = await base.post<UserResponseType>("/user/login", data, {
      schema: userResponse,
    });

    if (res.status === 200)
      authStore.setState((state) => ({
        ...state,
        isAuthenticated: true,
        user: res.data,
        error: null,
      }));
    else {
      authStore.setState((state) => ({ ...state, error: res.statusText }));
      throw new Error(res.statusText, { cause: res.data });
    }
  },

  logout: async () => {
    const res = await base.get("/user/logout");
    if (res.status === 204) {
      authStore.setState((state) => ({
        ...state,
        isAuthenticated: false,
        user: null,
        error: null,
      }));
    } else throw new Error(res.statusText, { cause: res.data });
  },
};

export const useAuth = () => {
  const authState = useSelector(authStore, (state) => state);
  return { ...authState, ...authActions };
};