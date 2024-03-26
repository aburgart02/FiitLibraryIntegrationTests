import { bookApi } from "src/api/BookApi";
import { imageApi } from "src/api/ImageApi";
import { librarianApi } from "src/api/LibrarianApi";
import { rubricApi } from "src/api/RubricApi";
import { userApi } from "src/api/UserApi";
import { loginApi } from "./LoginApi";

export const api = {
    book: bookApi,
    image: imageApi,
    librarian: librarianApi,
    rubric: rubricApi,
    user: userApi,
    login: loginApi,
};

export enum ApiRoutes {
    SummarySelect = "api/books/summary/select",
    SummaryGet = "api/books/summary",
    Books = "api/books",
    Export = "api/books/export",
    Checkout = "api/books/checkout",
    Enqueue = "api/books/enqueue",
    Return = "api/books/return",
    Images = "api/images",
    LibrariansSelect = "api/librarians/select",
    AccountLogin = "api/account/login",
    AccountRegister = "api/account/register",
    AccountValidatePassword = "api/account/validate",
    RubricSummarySelect = "api/rubrics/summary/select",
    RubricSummary = "api/rubrics/summary",
    User = "api/user",
    UserSignout = "api/user/signout"
}