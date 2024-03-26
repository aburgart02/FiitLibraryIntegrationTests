import * as React from "react";
import { RubricSummary } from "src/models/RubricSummary"

export const ModalHeader: React.FC<{ handleClose: () => void }> = ({ handleClose }) => {
    return (
        <div className="modal-header">
            <h5 className="modal-title">Добавить книгу</h5>
            <button type="button" className="close" onClick={handleClose}>
                <span>&times;</span>
            </button>
        </div>
    );
}

export const ModalFooter: React.FC<{ handleClose: () => void; handleCreateBook: () => Promise<void> }> = ({ handleClose, handleCreateBook }) => {
    return (
        <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={handleClose}>
                Закрыть
            </button>
            <button id='add-book-button' type="button" className="btn btn-primary" onClick={handleCreateBook}>
                Добавить
            </button>
        </div>
    );
}



export const BookInfoForm: React.FC<{
    name: string;
    author: string;
    nameError: boolean;
    authorError: boolean;
    handleInputChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void
}> = ({
    name,
    author,
    nameError,
    authorError,
    handleInputChange,
}) => (
    <div className="row">
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookName">Название книги</label>
                <input
                    type="text"
                    className={`form-control ${nameError ? "is-invalid" : ""}`}
                    id="bookName"
                    name="name"
                    value={name}
                    onChange={handleInputChange}
                    required
                />
                <div className="invalid-feedback">
                    {"Поле не может быть пустым и быть длиннее 20 символов"}
                </div>
            </div>
        </div>
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookAuthor">Автор</label>
                <input
                    type="text"
                    className={`form-control ${authorError ? "is-invalid" : ""}`}
                    id="bookAuthor"
                    name="author"
                    value={author}
                    onChange={handleInputChange}
                    required
                />
                <div className="invalid-feedback">
                    {"Поле не может быть пустым и быть длиннее 20 символов"}
                </div>
            </div>
        </div>
    </div>
);

export const BookDetailsForm: React.FC<{
    description: string;
    descriptionError: boolean;
    rubrics: RubricSummary[];
    rubricId: number;
    imageError: boolean;
    rubricError: boolean;
    handleInputChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
    handleImageUpload: (event: React.ChangeEvent<HTMLInputElement>) => Promise<void>;
}> = ({
    description,
    descriptionError,
    rubrics,
    rubricId,
    imageError,
    handleInputChange,
    handleImageUpload,
    rubricError
}) => (
    <div className="row">
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookDescription">Описание</label>
                <textarea
                    className={`form-control ${descriptionError ? "is-invalid" : ""}`}
                    id="bookDescription"
                    name="description"
                    value={description}
                    onChange={handleInputChange}
                />
                <div className="invalid-feedback">
                    {"Поле не может быть пустым и быть длиннее 20 символов"}
                </div>
            </div>
        </div>
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookRubricId">Рубрика</label>
                <select
                    className={`form-control ${rubricError ? "is-invalid" : ""}`}
                    id="bookRubricId"
                    name="rubricId"
                    value={rubricId}
                    onChange={handleInputChange}
                >
                    {rubrics.map((rubric) => (
                        <option key={rubric.name} value={rubric.id}>
                            {rubric.name}
                        </option>
                    ))}
                </select>
                <div className="invalid-feedback">
                    {
                        "Выберите рубрику"
                    }
                </div>
            </div>
        </div>
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookImageFile">Картинка</label>
                <input
                    className={`form-control ${imageError ? "is-invalid" : ""}`}
                    id="bookImageFile"
                    type="file"
                    name="formImageFile"
                    accept="image/*"
                    onChange={handleImageUpload}
                />
                <div className="invalid-feedback">
                    {
                        "Вы не выбрали изображение или оно не соответствует требованиям (формат jpg и не более 150КБ)"
                    }
                </div>
            </div>
        </div>
    </div>
);

export const BookCountPriceForm: React.FC<{
   
    price: string;
  
    priceError: boolean;
    handleInputChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
}> = ({
   
    price,
   
    priceError,
    handleInputChange,
}) => (
    <div className="row">
        <div className="col-md-10 mx-auto">
            <div className="form-group">
                <label htmlFor="bookPrice">Цена</label>
                <input
                    className={`form-control ${priceError ? "is-invalid" : ""}`}
                    id="bookPrice"
                    name="price"
                    value={price}
                    onChange={handleInputChange}
                />
                <div className="invalid-feedback">{"Цена не может быть пустой"}</div>
            </div>
        </div>
    </div>
);