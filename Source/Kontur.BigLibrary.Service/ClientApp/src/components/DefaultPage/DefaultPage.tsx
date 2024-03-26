import * as React from "react";
import Footer from "../Footer/Footer";
import Header from "../Header/Header";
const style = require("./DefaultPage.less");

interface DefaultPageProps {
    title: string;
}

export default class DefaultPage extends React.Component<DefaultPageProps> {
    componentDidMount(): void {
        document.title = this.props.title;
    }

    render(): React.ReactNode {
        return (
            <div className={style.wrapper}>
                <div className={style.page} id="root">
                    <div>
                        <Header/>
                        {this.props.children}
                    </div>
                    <Footer/>
                </div>
            </div>
        );
    }
}
