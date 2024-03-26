import * as React from "react";
import {Librarian} from "src/models/Librarian";
import {api} from "src/api/Api";
import {LibrarianContact} from "src/components/LibrarianContact/LibrarianContact";
import {Link} from "src/components/Link/Link";

const style = require("./Footer.less");

interface FooterState {
    librarians: Librarian[];
}

export default class Footer extends React.Component<{}, FooterState> {
    state: FooterState = {
        librarians: [],
    };

    componentDidMount(): void {
        this.loadLibrarians();
    }

    render(): React.ReactNode {
        return (
            <footer className={style.footer}>
                <h1 className={style.footerHeadline}>Вопросы по библиотеке</h1>
                <div className={style.footerBody}>
                    <div className={style.librarians}>
                        {this.state.librarians.map(librarian => this.renderLibrarianInfo(librarian))}
                    </div>
                    <div>
                        <div className={style.howItWorks}>
                            <Link to="/how-it-works" color="gray">
                                Как всё устроено?
                            </Link>
                        </div>
                        <div className={style.room}>
                            Все книги в комнате 202 <br/>
                            <span className={style.Maloprudnaya}>на Малопрудной, 5</span>
                        </div>
                    </div>
                </div>
            </footer>
        );
    }

    loadLibrarians = async () => {
        const librarians = await api.librarian.select();
        this.setState({librarians});
    };

    renderLibrarianInfo = (librarian: Librarian) => {
        const telegram = librarian.contacts.find(contact => contact.type === "telegram");
        const email = librarian.contacts.find(contact => contact.type === "email");

        return (
            <div key={librarian.id} className={style.librarian}>
                <div className={style.librarianName}>{librarian.name}</div>
                <div className={style.librarianTelegram}>
                    <LibrarianContact contact={telegram} gray/>
                </div>
                <div className="librarianEmail">
                    <LibrarianContact contact={email} gray/>
                </div>
            </div>
        );
    }
}
