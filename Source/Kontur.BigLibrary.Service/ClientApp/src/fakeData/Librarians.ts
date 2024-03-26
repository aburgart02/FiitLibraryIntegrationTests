import {Librarian} from "src/models/Librarian";

export const librarians: Librarian[] = [
    {
        "name": "Даша Сергеева",
        "contacts": [
            {
                "type": "telegram",
                "value": "dash_ka",
            },
            {
                "type": "email",
                "value": "korobicina@skbkontur.ru",
            },
            {
                "type": "phone",
                "value": "+7 912 628-00-79",
            },
        ],
        "id": 4,
        "isDeleted": false,
    }, {
        "name": "Оля Коновалова",
        "contacts": [
            {
                "type": "telegram", "value": "cat_in_cap",
            },
            {
                "type": "email",
                "value": "romanova@skbkontur.ru",
            },
            {
                "type": "phone", "value": "+7 982 658-33-36",
            },
        ],
        "id": 5,
        "isDeleted": false,
    }];
