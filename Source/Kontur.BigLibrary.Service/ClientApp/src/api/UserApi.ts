import { handleRequest } from "src/api/HandleRequest";
import { User } from "src/models/User";
import { ApiRoutes } from "./Api";

export const userApi = {
    get: async (): Promise<User> => {
        return handleRequest(ApiRoutes.User);
    },
    logout: async (): Promise<void> => {
        return handleRequest(ApiRoutes.UserSignout);
    },
};