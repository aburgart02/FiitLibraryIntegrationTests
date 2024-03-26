import * as React from "react";
import ArrowChevronLeftIcon from "@skbkontur/react-icons/ArrowChevron2Left";
import { Link } from "src/components/Link/Link";

export default function LinkBackToAllBooks(): JSX.Element {
    return (
        <span>
            <Link to="/" style={{display: "flex"}}>
                <ArrowChevronLeftIcon size={14} />
                Все книги
            </Link>
        </span>
    );
}
