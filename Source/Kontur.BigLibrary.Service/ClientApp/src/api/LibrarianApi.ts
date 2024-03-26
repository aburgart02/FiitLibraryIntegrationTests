import { Librarian } from "src/models/Librarian";
import { handleRequest } from "src/api/HandleRequest";
import { ApiRoutes } from "./Api";

export const librarianApi = {
    select: async (): Promise<Librarian[]> => {
        return handleRequest(ApiRoutes.LibrariansSelect);
    },
};
