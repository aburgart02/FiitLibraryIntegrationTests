import { AcceptType, handleRequest } from "src/api/HandleRequest";
import { JWTModel, PasswordValidationResult, UserLoginModel } from "src/models/UserLogin";
import { ApiRoutes } from "./Api";

export const loginApi = {
    login: async (bodyData: UserLoginModel): Promise<JWTModel> => {
        return handleRequest(ApiRoutes.AccountLogin, "POST", bodyData, AcceptType.Json, false);
    },

    register: async (bodyData: UserLoginModel): Promise<JWTModel> => {
        return handleRequest(ApiRoutes.AccountRegister, "POST", bodyData, AcceptType.Json, false);
    },

    validate: async (password: string): Promise<PasswordValidationResult> => {
        return handleRequest(`${ApiRoutes.AccountValidatePassword}?password=${password}`);
    },
};
