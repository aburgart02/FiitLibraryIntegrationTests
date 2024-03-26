import * as React from "react";
import * as InfiniteScroll from "react-infinite-scroller";
import { api } from "src/api/Api";
import { Link } from "react-router-dom";
import { BookFilter } from "src/models/BookFilter";
import ContentHeader from "src/components/ContentHeader/ContentHeader";
import Spinner from "@skbkontur/react-ui/Spinner";
import Center from "@skbkontur/react-ui/Center";
import BooksNotFoundBlock from "src/components/BooksNotFoundBlock/BooksNotFoundBlock";
import { BookSummary } from "src/models/BookSummary";
import StateLabel from "src/components/StateLabel/StateLabel";
import { Table } from "../Table";
import { IconButton } from "../IconButton";
import Gapped from "@skbkontur/react-ui/components/Gapped/Gapped";
import { CreateBookModal } from "../CreateBookModal/CreateBookModal";
import { setFilter } from "src/utils/LocalStorageFilters";

const classNames = require("classnames");
const styles = require("./BookList.less");

const count = 20;

const BooksList = ({ filter }: { filter: BookFilter }) => {
    const [books, setBooks] = React.useState<BookSummary[]>([]);
    const [showOnlyFree, setShowOnlyFree] = React.useState(false);
    const [hasMore, setHasMore] = React.useState(books.length>=count);
    const [isTable, setIsTable] = React.useState(false);
    const [showModal, setShowModal] = React.useState(false);

    React.useEffect(() => {
        setBooks([]);
        setHasMore(true);
    }, [filter.rubricSynonym, filter.query]);

    const loadData = async (start: number, showFree: boolean) => {
        const nextBooks = await api.book.select(
            start,
            count,
            filter,
            showFree ? false : null
        );

        setFilter({ filter, showFree: showFree ? false : null });

        setBooks((prevBooks) => [...prevBooks, ...nextBooks]);
        setHasMore(nextBooks.length === count);
    };

    const RenderBookPreview: React.FC<{ book: BookSummary }> = ({ book }) => (
        <Link data-tid={`bookItem-${book.synonym}`} to={`../books/${book.synonym}`} key={book.name + book.synonym}>
            <li data-tid="book-link" className={styles.wrapper}>
                <StateLabel isBusy={book.isBusy} />
                <img
                    className={classNames(styles.cover, { [styles.busy]: book.isBusy })}
                    src={api.image.getUrl(book.imageId)}
                    alt={book.synonym}
                />
            </li>
        </Link>
    );

    const RenderBookTablePreview: React.FC<{ book: BookSummary }> = ({ book }) => (
        <Table.Tr key={book.name + book.synonym}>
            <Table.Td>
                <Link to={`../books/${book.synonym}`}>{book.name}</Link>
            </Table.Td>
            <Table.Td>{book.author}</Table.Td>
            <Table.Td>{book.rubricName}</Table.Td>
            <Table.Td>{book.isBusy ? "занята" : "свободна"}</Table.Td>
            <Table.Td>{book.price ?? ""}</Table.Td>
        </Table.Tr>
    );

    const renderContentHeader = () => {
        if (filter.rubricSynonym) {
            return (
                <ContentHeader
                    showOnlyFree={showOnlyFree}
                    rubricSynonym={filter.rubricSynonym}
                    onShowFreeClick={handleShowFree}
                    additionalContent={
                        <Gapped>
                            <IconButton
                                dataTid="book-list-change-view"
                                onClick={() => setIsTable(!isTable)}
                                icon={isTable ? "ListRows" : "ListGroup"}
                            />
                            <IconButton
                                dataTid="book-add"
                                icon={"Add"}
                                onClick={() => setShowModal(true)}
                            />
                        </Gapped>
                    }
                />
            );
        }
        if (!filter.query) {
            return (
                <ContentHeader
                    showOnlyFree={showOnlyFree}
                    bigText="Все книги"
                    onShowFreeClick={handleShowFree}
                    additionalContent={
                        <Gapped>
                            <IconButton
                                dataTid="book-list-change-view"
                                onClick={() => setIsTable(!isTable)}
                                icon={isTable ? "ListRows" : "ListGroup"}
                            />
                            <IconButton
                                dataTid="book-add"
                                icon={"Add"}
                                onClick={() => setShowModal(true)}
                            />
                        </Gapped>
                    }
                />
            );
        }
        if (books.length !== 0) {
            return (
                <ContentHeader
                    showOnlyFree={showOnlyFree}
                    smallText="Вот что нашли:"
                    onShowFreeClick={handleShowFree}
                    additionalContent={
                        <Gapped>
                            <IconButton
                                dataTid="book-list-change-view"
                                onClick={() => setIsTable(!isTable)}
                                icon={isTable ? "ListRows" : "ListGroup"}
                            />
                            <IconButton
                                dataTid="book-add"
                                icon={"Add"}
                                onClick={() => setShowModal(true)}
                            />
                        </Gapped>
                    }
                />
            );
        }
        return null;
    };

    const handleShowFree = async () => {
        setShowOnlyFree((prevShowOnlyFree) => !prevShowOnlyFree);
        setBooks([]);
        setHasMore(true);
    };

    return (
        <div className={styles.bookList}>
            {books.length === 0 && !hasMore ? (
                <div className={styles.bookList}>
                    <IconButton
                        dataTid=""
                        icon={"Add"}
                        onClick={() => setShowModal(true)}
                    />
                    <BooksNotFoundBlock />
                    <CreateBookModal
                        onClose={() => setShowModal(false)}
                        show={showModal}
                    />
                </div>
            ) : (
                <div data-tid="book-list" className={styles.bookList}>
                    {renderContentHeader()}
                    <InfiniteScroll
                        loadMore={() => loadData(books.length, showOnlyFree)}
                        hasMore={hasMore}
                        loader={
                            <Center key={books.length}>
                                <Spinner type={"big"} />
                            </Center>
                        }
                    >
                        {isTable ? (
                            <div className={styles.tableWrapper}>
                                <Table data-ft-id='books-table'>
                                    <Table.Thead>
                                        <Table.Tr>
                                            <Table.Th>Название</Table.Th>
                                            <Table.Th>Автор</Table.Th>
                                            <Table.Th>Рубрика</Table.Th>
                                            <Table.Th>Статус</Table.Th>
                                            <Table.Th>Цена</Table.Th>
                                        </Table.Tr>
                                    </Table.Thead>
                                    <Table.Tbody>
                                        {books.map((x, index) => <RenderBookTablePreview book={x} key={x.name + index} />)}
                                    </Table.Tbody>
                                </Table>
                            </div>
                        ) : (
                            books.map((x, index) => <RenderBookPreview book={x} key={x.name + index} />)
                        )}
                    </InfiniteScroll>
                    <CreateBookModal
                        onClose={() => setShowModal(false)}
                        show={showModal}
                    />
                </div>
            )}
        </div>
    );
};

export default BooksList;
