export interface Librarian {
    id: number;
    name: string;
    contacts: Contact[];
    isDeleted: boolean;
}

export interface Contact {
    type: string;
    value: string;
}