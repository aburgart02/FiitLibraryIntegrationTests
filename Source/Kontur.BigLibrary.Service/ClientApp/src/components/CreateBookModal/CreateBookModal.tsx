import * as React from "react";
import { api } from "src/api/Api";
import { Book } from "src/models/Book";
import { RubricSummary } from "src/models/RubricSummary";
import { ModalHeader, BookInfoForm, BookDetailsForm, BookCountPriceForm, ModalFooter } from "./FormParts";
import { handleImageUpload, handleInputChange, loadRubrics } from "./helper";

interface Props {
    show: boolean;
    onClose: () => void;
}

const initialState = {
    name: "",
    author: "",
    description: "",
    rubricId: -1,
    imageId: -1,
    price: "0",
    count: 1
};

export const CreateBookModal: React.FC<Props> = ({ onClose, show }) => {
    const [book, setBook] = React.useState<Omit<Book, "id">>(initialState);

    const [nameError, setNameError] = React.useState<boolean>(false);
    const [descriptionError, setDescriptionError] = React.useState<boolean>(false);
    const [authorError, setAuthorError] = React.useState<boolean>(false);
    const [imageError, setImageError] = React.useState<boolean>(false);
    const [priceError, setPriceError] = React.useState<boolean>(false);
    const [rubricError, setRubricError] = React.useState<boolean>(false);


    const [rubrics, setRubrics] = React.useState<RubricSummary[]>([]);

    const handleClose = (): void => {
        const inputElement = document.getElementById("bookImageFile") as HTMLInputElement | null;
        if (inputElement) {
            inputElement.value = "";
        }
        onClose();
    };

    React.useEffect(() => {
        void loadRubrics(setBook, setRubrics);
    }, []);

    const handleCreateBook = async (): Promise<void> => {
        resetErrors();

        if (!validate(book.name)) {
            setNameError(true);
        }

        if (!validate(book.author)) {
            setAuthorError(true);
        }

        if (!validate(book.description)) {
            setDescriptionError(true);
        }

        if (book.imageId < 0) {
            setImageError(true);
        }


        if (!book.price) {
            setPriceError(true);
        }

        if (book.rubricId < 0) {
            setRubricError(true);
        }

        if (validate(book.name) && validate(book.author) && validate(book.description) && !(book.imageId < 0) && !(book.rubricId < 0)) {
            await api.book.save(book);
            setBook({ ...initialState, rubricId: rubrics[0].id });

            handleClose();
        }
    };

    const validate = (value: string): boolean => {
        if (value === "" || value.length > 20) {
            return false;
        }

        return true;
    };

    const resetErrors = (): void => {
        setNameError(false);
        setAuthorError(false);
        setDescriptionError(false);
        setImageError(false);
    };

    return (
        <div className={`modal ${show ? "show" : ""}`} style={{ display: show ? "block" : "none" }}>
            <div className="modal-dialog modal-lg modal-dialog-scrollable">
                <div id='create-book-lightbox' className="modal-content">
                    <ModalHeader handleClose={handleClose} />
                    <div className="modal-body">
                        <form>
                            <BookInfoForm
                                name={book.name}
                                author={book.author}
                                nameError={nameError}
                                authorError={authorError}
                                handleInputChange={(e) => handleInputChange(e, setBook)}
                            />
                            <BookDetailsForm
                                description={book.description}
                                descriptionError={descriptionError}
                                rubrics={rubrics}
                                rubricId={book.rubricId}
                                imageError={imageError}
                                rubricError={rubricError}
                                handleInputChange={(e) => handleInputChange(e, setBook)}
                                handleImageUpload={(e) => handleImageUpload(e, setBook, setImageError)}
                            />
                            <BookCountPriceForm
                                price={book.price}
                                priceError={priceError}
                                handleInputChange={(e) => handleInputChange(e, setBook)}
                            />
                        </form>
                    </div>
                    <ModalFooter handleClose={handleClose} handleCreateBook={handleCreateBook} />
                </div>
            </div>
        </div>
    );
};
