import * as React from "react";
const style = require("./HowItWorks.less");
import Link from "@skbkontur/react-ui/Link";
import LinkBackToAllBooks from "../LinkBackToAllBooks/LinkBackToAllBooks";
import {Librarian} from "src/models/Librarian";
import {LibrarianContact} from "src/components/LibrarianContact/LibrarianContact";

interface HowItWorksProps {
    cards: Librarian[];
}

export default function HowItWorks({ cards }: HowItWorksProps): React.ReactElement {
    return (
        <div className={style.pageHowItWorks}>
            <LinkBackToAllBooks />
            <Info />
            <Cards cards={cards} />
        </div>
    );
}

function Info(): React.ReactElement {
    return (
        <div className={style.info}>
            <h1>Как всё устроено</h1>
            <div>Все книги живут в 202 комнате на Малопрудной, 5. Там есть уютный уголок, в котором можно посидеть и почитать. Приходи, выбирай, читай.</div>

            <h2>Взять книгу</h2>
            <p>
                Чтобы взять книгу с полки — запишись через <Link target="_blank" href="https://t.me/BigLibrary_bot">бота</Link> или <Link target="_blank" href="https://staff.skbkontur.ru/comments?m=article-5aec2231339efc063c37e823">на листочке</Link> в библиотеке. Листок лежит на полочке возле ноутбука. Читать можно не дольше 2 месяцев.
            </p>
            <p>
                Книги из коробки и с полки у входа не бери, их разбирает библиотекарь и следит, есть ли за книгой очередь.
            </p>

            <h2>Сдать книгу</h2>
            <p>
                Сдавай книги в коробку около входа с надписью «Сдавай сюда» — напиши своё имя на стикере. Библиотекарь спишет её с тебя, положит на место или отложит для читателя из очереди.
            </p>
            <p>
                Не бери книги из этой коробки, их разбирает только библиотекарь.
            </p>

            <h2>Другие офисы Екатеринбурга</h2>
            <p>
                <i>Взять книгу.</i> Если ты из другого офиса, в <Link target="_blank" href="https://t.me/BigLibrary_bot">боте</Link> около нужной книги жми «Получить книгу». Далее следуй подсказкам бота. Книга будет ждать на полке 7 дней.
            </p>
            <p>
                <i>Сдать книгу.</i> Отправь книгу на Малопрудную внутренней почтой:
            </p>
            <p>
                <ol>
                    <li>Подпиши на стикере свою фамилию, и что книга в Большую библиотеку.</li>
                    <li>Упакуй книгу в файлик.</li>
                    <li>Найди офис-менеджера в своем офисе и передай книгу для отправки.</li>
                </ol>
            </p>

            <h2>Заказать книгу</h2>
            <p>
                Если ты не нашёл нужной книги в нашей библиотеке, можно оставить заявку в <Link target="_blank" href="https://t.me/BigLibrary_bot">боте</Link> или написать <Link target="_blank" href="https://staff.skbkontur.ru/profile/yanasak">Яне</Link>.
                Мы покупаем книги близкие к профессиональной тематике, которые будут развивать тебя. Несколько художественных книг, которые есть в библиотеке — скорее исключение, мы не покупаем художественную литературу.
            </p>

            <h2>Буккросинг</h2>
            <p>
                Книги буккросинга стоят на белых стеллажах, которые отделяют уютный уголок. Их можно брать без записи, там правил нет. Приносить из дома тоже можно.
            </p>

            <h2>Общайся</h2>
            <p>
                {"В Стаффе у нас есть сообщество "}
                <Link target="_blank" href="https://staff.skbkontur.ru/group/6756">
                    Большая библиотека Контура
                </Link>
                {". В нём новости библиотеки и другие полезные штуки. Будем рады, если ты напишешь отзыв о книге."}
            </p>

            <h2>Кто библиотекарь?</h2>
            <p>
                {"Сейчас в библиотеке по очереди дежурят 4 библиотекаря: "}
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/alinga">
                    Алина Галиулина
                </Link>
                {", "}
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/dboiar">
                    Даша Боярских
                </Link>
                {", "}
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/sokolova">
                    Оля Маркина
                </Link>
                {", "}
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/foxyslan">
                    Света Руднева
                </Link>
                {
                    ". Они ежедневно разбирают книги, которые сдали читатели, записывают в базу тех, кто записался на листочке в библиотеке, пишут вам о том, что подошла очередь за книгой."
                }
            </p>
            <p>
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/arina_ra">
                    Арина Разгоняева
                </Link>
                {" следит за порядкои и чтобы должники сдавали книги."}
                <br />
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/mvs">
                    Вика Мирошниченко
                </Link>
                {" —  дизайнер, публикует анонсы о новых книгах."}
                <br />
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/e_pajl">
                    Катя Пайль
                </Link>
                {" повелевает ботом, в нём постоянно нужно что-то докручивать, улучшать."}
                <br />
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/romanova">
                    Оля Коновалова
                </Link>
                {" — создатель и идейный вдохновитель и просто волшебница."}
            </p>

            <h2>Кому задать вопрос?</h2>
            <p>
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/foxyslan">Света Руднева</Link> отвечает на любые ваши вопросы, следит за тем, чтобы должники сдавали книги. Смело задавай вопросы Свете.
            </p>
            <p>
                <Link target="_blank" href="https://staff.skbkontur.ru/profile/yanasak">Яна Сак</Link> занимается организационными вопросами, закупкой книг, думает, как сделать жизнь читателей и библиотекарей проще и лучше, следит за тем, чтобы всё работало, как надо. Пиши Яне предложения по улучшению библиотеки.
            </p>
        </div>
    );
}

function Cards({cards}: HowItWorksProps): JSX.Element {
    return (
        <div className={style.cards}>
            {cards.map(card => (
                <div key={card.id}>{renderCard(card)}</div>
            ))}
        </div>
    );
}

function renderCard({ name, contacts }: Librarian): JSX.Element {
    const telegram = contacts.find(contact => contact.type === "telegram");
    const email = contacts.find(contact => contact.type === "email");
    const phone = contacts.find(contact => contact.type === "phone");

    return (
        <div className={style.card}>
            <h3 className={style.cardHeader}>{name}</h3>
            <div className={style.infoCard}>
                <LibrarianContact contact={telegram}/>
            </div>
            <div className={style.infoCard}>
                <LibrarianContact contact={email}/>
            </div>
            <div className={style.infoCard}>
                <LibrarianContact contact={phone}/>
            </div>
        </div>
    );
}
