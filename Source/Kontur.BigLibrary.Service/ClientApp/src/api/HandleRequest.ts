import { NotFoundError, ServerError } from "src/api/Errors";
import fetch, { RequestInfo, RequestInit } from "node-fetch";
import { getJwt, removeJwt } from "src/utils/LocalStorageJWT";

export enum AcceptType {
    Json = "json",
    Xml = "xml",
}

export enum HttpStatus {
    Unauthorized = 401,
    NotFound = 404,
}

export const handleRequest = async <T>(
    input: RequestInfo,
    method: string = "GET",
    bodyData?: unknown,
    acceptType: AcceptType = AcceptType.Json,
    needReload: boolean = true,
): Promise<T> => {
    const jwtToken = getJwt();

    const headers: Record<string, string> = {
        Authorization: `Bearer ${jwtToken}`,
        "Content-Type": "application/json",
    };

    if (acceptType === AcceptType.Xml) {
        headers.Accept = "application/xml";
    }

    const requestOptions: RequestInit = {
        method,
        headers,
        body: bodyData ? JSON.stringify(bodyData) : undefined,
    };

    const response = await fetch(input, requestOptions);

    if (response.ok) {
        switch (acceptType) {
            case AcceptType.Json:
                return response.json();
            case AcceptType.Xml:
                return response.text() as T;
            default:
                return null;
        }
    } else {
        const errorMessage = await response.text();
        switch (response.status) {
            case HttpStatus.Unauthorized:
                removeJwt();
                if (needReload) {
                    window.location.reload();
                }
                return errorMessage as T;
            case HttpStatus.NotFound:
                throw new NotFoundError(errorMessage);
            default:
                throw new ServerError(errorMessage);
        }
    }
}
