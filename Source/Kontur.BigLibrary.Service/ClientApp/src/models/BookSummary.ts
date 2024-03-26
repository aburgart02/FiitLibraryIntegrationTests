import {Book} from "src/models/Book";

export interface BookSummary extends Book {
    rubricName: string;
    rubricSynonym: string;
    isBusy: boolean;
    synonym: string;
}
