import * as React from "react";
import BookView from "../BookView/BookView";
import DefaultPage from "../DefaultPage/DefaultPage";
import {api} from "src/api/Api";
import Loader from "@skbkontur/react-ui/Loader";
import {BookSummary} from "src/models/BookSummary";

interface BookViewPageProps {
    synonym: string;
}

interface BookViewPageState {
    bookSummary: BookSummary;
    imgUrl: string;
    loading: boolean;
}

export default class BookViewPage extends React.Component<BookViewPageProps, BookViewPageState> {
    state: BookViewPageState = {
        bookSummary: null,
        imgUrl: "",
        loading: true,
    };

    componentDidMount(): void {
        this.loadData(this.props.synonym);
    }

    render(): React.ReactNode {
        return (
            <>
                {this.state.loading && (<Loader type="normal" active/>)}
                {!this.state.loading && (
                    <DefaultPage title={this.state.bookSummary.name}>
                        <BookView
                            bookSummary={this.state.bookSummary}
                            imgUrl={this.state.imgUrl}
                        />
                    </DefaultPage>
                )}
            </>
        );

    }

    loadData = async (synonym: string) => {
        const bookSummary = await api.book.get(synonym);
        const imgUrl = api.image.getUrl(bookSummary.imageId, "l");
        this.setState({bookSummary, imgUrl, loading: false});
    };
}
