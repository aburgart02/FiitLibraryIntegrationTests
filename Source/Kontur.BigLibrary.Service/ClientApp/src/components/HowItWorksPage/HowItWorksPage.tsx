import * as React from "react";
import HowItWorks from "../HowItWorks/HowItWorks";
import DefaultPage from "../DefaultPage/DefaultPage";
import { Librarian } from "src/models/Librarian";
import { api } from "src/api/Api";

interface HowItWorksPageState {
    librarians: Librarian[];
}

export default class HowItWorksPage extends React.Component<{}, HowItWorksPageState> {
    state: HowItWorksPageState = {
        librarians: [],
    };

    componentDidMount(): void {
        this.loadLibrarians();
    }

    render(): React.ReactNode {
        return (
            <DefaultPage title={"Как всё устроено"}>
                <HowItWorks cards={this.state.librarians} />
            </DefaultPage>
        );
    }

    loadLibrarians = async () => {
        const librarians = await api.librarian.select();
        this.setState({ librarians });
    };
}
