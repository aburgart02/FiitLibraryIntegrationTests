import * as React from "react";
const Clouds = require("./clouds.svg");
const style = require("./ErrorPage.less");

interface ErrorPageProps {
    text: string;
    code: number;
}

export default class ErrorPage extends React.Component<ErrorPageProps> {
    componentDidMount(): void {
        document.title = this.props.text;
    }

    render(): React.ReactNode {
        return (
            <div className={style.errorPage}>
                <div className={style.info}>
                    <br />
                    <br />
                    <h2 className={style.library}>Библиотека</h2>
                    <br />
                    <br />
                    <h1 className={style.errorText}>
                        {this.props.text}
                        <sup className={style.errorCode}>{this.props.code}</sup>
                    </h1>
                    {this.props.children}
                </div>

                <div className={style.clouds}>
                    <svg
                        id={style.far}
                        className={style.clouds__item}
                        width="5540"
                        height="667"
                        viewBox="0 0 5540 567"
                        xmlns="http://www.w3.org/2000/svg">
                        <use xlinkHref="#clouds-far" x="0" y="0"></use>
                        <use xlinkHref="#clouds-far" x="2770" y="0"></use>
                    </svg>

                    <svg
                        id={style.med}
                        className={style.clouds__item}
                        width="5540"
                        height="667"
                        viewBox="0 0 5540 567"
                        xmlns="http://www.w3.org/2000/svg">
                        <use xlinkHref="#clouds-med" x="0" y="175"></use>
                        <use xlinkHref="#clouds-med" x="2770" y="175"></use>
                    </svg>

                    <svg
                        id={style.near}
                        className={style.clouds__item}
                        width="5540"
                        height="667"
                        viewBox="0 0 5540 567"
                        xmlns="http://www.w3.org/2000/svg">
                        <use xlinkHref="#clouds-near" x="0" y="0"></use>
                        <use xlinkHref="#clouds-near" x="2770" y="0"></use>
                    </svg>
                </div>

                <Clouds />
            </div>
        );
    }
}
