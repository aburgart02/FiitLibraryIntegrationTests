import {Librarian} from "src/models/Librarian";
import {librarians} from "src/fakeData/Librarians";

const getLibrarians = (): Promise<Librarian[]> => {
    return new Promise(resolve => {
        setTimeout(() => resolve(librarians), 500);
    });
};

export const fakeLibrariansApi = {
    select: getLibrarians,
};
