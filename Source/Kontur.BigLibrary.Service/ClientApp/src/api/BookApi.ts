import { BookFilter } from "src/models/BookFilter";
import * as querystring from "querystring";
import { AcceptType, handleRequest } from "src/api/HandleRequest";
import { BookSummary } from "src/models/BookSummary";
import { Book } from "src/models/Book";
import { ApiRoutes } from "./Api";
import { getFilter } from "src/utils/LocalStorageFilters";

export const bookApi = {
    select: async (start: number, count: number, filter: BookFilter, isBusy?: false | null): Promise<BookSummary[]> => {
        const searchParams = querystring.stringify({ ...filter, limit: count, offset: start, isBusy });
        return handleRequest(`${ApiRoutes.SummarySelect}?${searchParams}`);
    },
    get: async (synonym: string): Promise<BookSummary> => {
        return handleRequest(`${ApiRoutes.SummaryGet}/${synonym}`);
    },
    save: async (data: Omit<Book, "id">): Promise<Book> => {
        return handleRequest(ApiRoutes.Books, "POST", data);
    },
    export: async () => {
        const filterContainer = getFilter();
        const searchParams = querystring.stringify({ ...filterContainer.filter, isBusy: filterContainer.showFree });
        return handleRequest(`${ApiRoutes.Export}?${searchParams}`, "GET", null, AcceptType.Xml);
    },
    checkout: async (bookId: number, userName: string): Promise<{ message: string }> => {
        const searchParams = querystring.stringify({ bookId, userName });
        return handleRequest(`${ApiRoutes.Checkout}?${searchParams}`);
    },
    enqueue: async (bookId: number, userName: string): Promise<{ message: string }> => {
        const searchParams = querystring.stringify({ bookId, userName });
        return handleRequest(`${ApiRoutes.Enqueue}?${searchParams}`);
    },
    return: async (bookId: number, userName: string): Promise<{ message: string }> => {
        const searchParams = querystring.stringify({ bookId, userName });
        return handleRequest(`${ApiRoutes.Return}?${searchParams}`);
    },
};