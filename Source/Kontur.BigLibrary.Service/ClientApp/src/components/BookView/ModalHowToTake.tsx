import * as React from "react";
import Modal from "@skbkontur/react-ui/Modal";
import { Link } from "../Link/Link";
const style = require("./ModalHowToTake.less");

export default function ModalHowToTake(): React.ReactElement {
    const [isOpen, setModalState] = React.useState(false);

    return (
        <span className={style.howToTake}>
            {isOpen && renderModal(() => setModalState(false))}
            <Link to={"#"} onClick={() => setModalState(true)}>Как взять книгу?</Link>
        </span>
    );

    function renderModal(closeModal: () => void): React.ReactNode {
        return (
            <Modal onClose={closeModal} width={"500px"}>
                <Modal.Header>Как взять книгу</Modal.Header>
                <Modal.Body>
                    <p>Библиотека находится в 202  комнате на Малопрудной, 5. Книга стоит на полке, которая соответствует её рубрике.</p>
                    <p>Запиши книгу на себя через <Link to={"#"} onClick={() => window.open("https://t.me/BigLibrary_bot","_blank")} >бота</Link> или <Link to={"#"} onClick={() => window.open("https://staff.skbkontur.ru/comments?m=article-5aec2231339efc063c37e823","_blank")} >на листочке</Link> в библиотеке. Листок лежит на полке возле ноутбука. Читать можно не дольше 2 месяцев.</p>
                    <p>Книги из коробки и с полки у входа не бери, их разбирает библиотекарь и следит, есть ли за книгой очередь.</p>
                    <p>Как сдать книгу, заказать новую, где узнавать новости и кому задавать вопросы — читай на страничке <Link to={"/how-it-works"} >Как всё устроено</Link>.</p>
                </Modal.Body>
                <Modal.Footer>
                    <button type="button" className="btn btn-primary" onClick={closeModal}>Хорошо</button>
                </Modal.Footer>
            </Modal>
        );
    }
}
