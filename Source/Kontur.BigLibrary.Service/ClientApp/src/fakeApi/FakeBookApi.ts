import {Book} from "src/models/Book";
import {allBooks, booksForQuery, booksForRubric} from "src/fakeData/Books";
import {book} from "src/fakeData/Book";
import {BookFilter} from "src/models/BookFilter";

const getAllBooks = (start: number, count: number, filter: BookFilter): Promise<Book[]> => {
    if (filter.rubricSynonym)
        return new Promise(resolve =>
            setTimeout(() => resolve(booksForRubric), 500));
    if (filter.query)
        return new Promise(resolve =>
            setTimeout(() => resolve(booksForQuery), 500));
    return new Promise(resolve =>
        setTimeout(() => resolve(allBooks.slice(start, start + count)), 500));
};

const getBook = (synonym: string): Promise<Book> => {
    return new Promise(resolve => {
        setTimeout(() => resolve(book), 500);
    });
};

export const fakeBookApi = {
    select: getAllBooks,
    get: getBook,
};
