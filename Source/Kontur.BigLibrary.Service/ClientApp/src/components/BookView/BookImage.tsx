import * as React from "react";
import Modal from "@skbkontur/react-ui/Modal";
const style = require("./BookView.less");

interface BookImageProps {
    imageUrl: string;
}

export default function BookImage({imageUrl}: BookImageProps): React.ReactElement {
    const [isModalOpen, setModalState] = React.useState(false);

    return (
        <div>
            <img className={style.img} src={imageUrl} onClick={() => setModalState(true)}/>
            {isModalOpen ? (
                <Modal onClose={() => setModalState(false)} noClose={true}>
                    <Modal.Body>
                        <img src={imageUrl} alt="book cover"/>
                    </Modal.Body>
                </Modal>
            ) : null}
        </div>
    );
}
