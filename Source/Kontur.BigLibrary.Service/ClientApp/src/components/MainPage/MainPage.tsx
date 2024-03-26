import * as React from "react";
import BooksList from "../BooksList/BooksList";
import DefaultPage from "../DefaultPage/DefaultPage";
import {BookFilter} from "src/models/BookFilter";

export interface MainPageProps {
    filter: BookFilter;
}

export default function MainPage(props: MainPageProps): React.ReactElement {
    return (
        <DefaultPage title={"Список книг"}>
            <BooksList filter={props.filter}/>
        </DefaultPage>
    );
}
