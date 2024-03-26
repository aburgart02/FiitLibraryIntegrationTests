import * as React from "react";

const booksNotFoundImage = require("./booksNotFoundImage.png");
const styles = require("./BooksNotFoundBlock.less");

const BooksNotFoundBlock = (): React.ReactElement => {
    return (
        <div className={styles.container}>
            <span className={styles.block}>
                <img src={booksNotFoundImage} alt={"Такой книги у нас нет"}/>
                <div className={styles.text}>Такой книги у нас нет</div>
            </span>
        </div>
    );
};

export default BooksNotFoundBlock;
