import * as React from "react";
import LinkBackToAllBooks from "../LinkBackToAllBooks/LinkBackToAllBooks";
import ModalHowToTake from "./ModalHowToTake";
import BookImage from "./BookImage";
import { Link } from "src/components/Link/Link";
import StateLabel from "src/components/StateLabel/StateLabel";
import { BookSummary } from "src/models/BookSummary";
import Gapped from "@skbkontur/react-ui/components/Gapped/Gapped";
import { api } from "src/api/Api";
import { User } from "src/models/User";
import MessageAlert, { MessageAlertProps } from "../MessageAlert/MessageAlert";
const style = require("./BookView.less");

interface BookViewProps {
    bookSummary: BookSummary;
    imgUrl: string;
}

export default function BookView(props: BookViewProps): React.ReactElement {
    const [user, setUser] = React.useState<User>({} as User);
    const [bookSummary, setBookSummary] = React.useState<BookSummary>(props.bookSummary);
    const [messageAlert, setMessageAlert] = React.useState<MessageAlertProps>({ message: "", type: "success" });

    const checkoutBook = async () => {
        try {
            const result = await api.book.checkout(bookSummary.id, user.name);
            const alertType = result.message === "Вы взяли книгу." ? "success" : "danger";
            result.message === "Вы взяли книгу." && setBookSummary(bs => ({ ...bs, isBusy: true }));
            setMessageAlert({ message: result.message, type: alertType });
        } catch (e) {
            console.error(e);
        }
    };

    const returnBook = async () => {
        try {
            const result = await api.book.return(bookSummary.id, user.name);
            const alertType = result.message === "Книга свободна." ? "success" : "danger";
            result.message === "Книга свободна." && setBookSummary(bs => ({ ...bs, isBusy: false }));
            setMessageAlert({ message: result.message, type: alertType });
        } catch (e) {
            console.error(e);
        }
    };

    const enqueue = async () => {
        try {
            const result = await api.book.enqueue(bookSummary.id, user.name);
            const alertType = result.message === "Вы встали в очередь." ? "success" : "danger";
            setMessageAlert({ message: result.message, type: alertType });
        } catch (e) {
            console.error(e);
        }
    };

    const loadUser = async () => {
        const currentUser = await api.user.get();
        setUser(currentUser);
    };

    React.useEffect(() => {
        void loadUser();
    }, []);

    return (
        <div className={style.bookView}>
            <MessageAlert message={messageAlert.message} type={messageAlert.type} />
            <Gapped>
                <LinkBackToAllBooks />
                <button type="button" className="btn btn-primary" onClick={checkoutBook}>Взять книгу</button>
                <button type="button" className="btn btn-primary" onClick={returnBook}>Вернуть книгу</button>
                <button type="button" className="btn btn-primary" onClick={enqueue}>Встать в очередь за книгой</button>
            </Gapped>
            <div className={style.bookAbout}>
                <div className={style.picColumn}>
                    <StateLabel isBusy={bookSummary.isBusy} />
                    <BookImage imageUrl={props.imgUrl} />
                    <ModalHowToTake />
                </div>
                <div className={style.info}>
                    <h1 className={style.name}>{bookSummary.name}</h1>
                    <div className={style.author}>{bookSummary.author}</div>
                    <div className={style.description}>{bookSummary.description}</div>
                    <div className={style}>
                        <span>Рубрика: </span>
                        <Link to={`/rubric/${bookSummary.rubricSynonym}`}>{bookSummary.rubricName}</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
