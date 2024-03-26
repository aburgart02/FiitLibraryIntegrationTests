import * as React from "react";
import ErrorPage from "./ErrorPage";
import Link from "@skbkontur/react-ui/Link";

export function NotFoundPage(): React.ReactElement {
    return (
        <ErrorPage code={404} text="Страница не найдена">
            <p>Возможно, вы ошиблись в адресе или такой страницы больше нет.</p>
            <p>
                Если адрес страницы верный, напишите нам на{" "}
                <Link href="mailto:library@kontur.ru">library@kontur.ru</Link>
            </p>
            <br />
            <br />
            <Link href="/">Перейти к списку книг</Link>
        </ErrorPage>
    );
}
