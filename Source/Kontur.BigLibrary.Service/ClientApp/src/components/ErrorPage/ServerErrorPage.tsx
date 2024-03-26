import * as React from "react";
import ErrorPage from "./ErrorPage";
import Link from "@skbkontur/react-ui/Link";

export function ServerErrorPage(): JSX.Element {
    return (
        <ErrorPage code={500} text="Сервис недоступен">
            <p>Извините, мы устраняем неполадки в сервисе.</p>
            <p>
                Если у вас есть вопросы, напишите нам на <Link href="mailto:library@kontur.ru">library@kontur.ru</Link>
            </p>
        </ErrorPage>
    );
}
