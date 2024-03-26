-- Данный скрипт уже применен к biglibrary.db
-- Поменял скрипт - пересоздай БД

-- select setval('book_rubric_id_seq', 1);

insert into book_rubric(name, is_deleted, order_id, parent_id) values
('Разработка', false, 0 , -1),
('Аналитика', false, 1, -1),
('Дизайн', false, 2, -1),
('Тестирование', false, 3, -1),
('Человек', false, 4, -1),
('Процессы и команда', false, 5, -1),
('Маркетинг', false, 6, -1),
('Наука', false, 7, -1),
('Прочее', false, 8, -1);

-----------------------Разработка-------------------------------
insert into book_rubric(name, is_deleted, order_id) values
('Базы данных и память', false, 9),
('Администрирование', false, 10),
('Языки программирования', false, 11),
('Мобильная разработка', false, 12),
('Фреймворки и форматы', false, 13),
('Безопасность', false, 14),
('Операционные системы',false, 15),
('Computer Science', false, 16),
('Архитектура и паттерны', false, 17),
('DevOps', false, 18),
('Стать звездой', false, 19),
('Data-Intensive Apps', false, 20),
('Верстка', false, 21),
('Мануалы от Intel', false, 22),
-----------------------Дизайн-------------------------------
('Дизайн', false, 23),
('Иллюстрация и рисунок', false, 24),
('Инфографика', false, 25),
('Проектирование интерфейсов', false, 26),
('Исследования', false, 27),
('Типографика', false, 28),
('Городская среда', false, 29),
-----------------------Тестирование-------------------------------
('Тестирование', false, 30),
('Автотесты', false, 31),
-----------------------Аналитика-------------------------------
('Аналитика', false, 32),
-----------------------Человек-------------------------------
('Психология и саморазвитие', false, 33),
('Эффективная работа', false, 34),
('Нейронаука и эволюция', false, 35),
-----------------------Процессы и команда-------------------------------
('Технологии', false, 36),
('Управление проектами', false, 37),
('Управление командами', false, 38),
-----------------------Маркетинг-------------------------------
('Маркетинг', false, 39),
('Редактура', false, 40),
-----------------------Наука-------------------------------
('Наука', false, 41),
('Математика и статистика', false, 42),
-----------------------Прочее-------------------------------
('Прочее', false, 43);

insert into book_rubric_index(id, synonym) select id, 'Parent_Razrabotka' from book_rubric where parent_id =-1 and name = 'Разработка';
insert into book_rubric_index(id, synonym) select id, 'Parent_Analitika' from book_rubric where parent_id =-1 and name = 'Аналитика';
insert into book_rubric_index(id, synonym) select id, 'Parent_Dizayn' from book_rubric where parent_id =-1 and name = 'Дизайн';
insert into book_rubric_index(id, synonym) select id, 'Parent_Testirovanie' from book_rubric where parent_id =-1 and name = 'Тестирование';
insert into book_rubric_index(id, synonym) select id, 'Parent_Chelovek' from book_rubric where parent_id =-1 and name = 'Человек';
insert into book_rubric_index(id, synonym) select id, 'Parent_Processy_i_komanda' from book_rubric where parent_id =-1 and name = 'Процессы и команда';
insert into book_rubric_index(id, synonym) select id, 'Parent_Marketing' from book_rubric where parent_id =-1 and name = 'Маркетинг';
insert into book_rubric_index(id, synonym) select id, 'Parent_Nauka' from book_rubric where parent_id =-1 and name = 'Наука';
insert into book_rubric_index(id, synonym) select id, 'Parent_Prochee' from book_rubric where parent_id =-1 and name = 'Прочее';
insert into book_rubric_index(id, synonym) select id, 'Bazy_dannykh_i_pamyat' from book_rubric where parent_id is null and name = 'Базы данных и память';
insert into book_rubric_index(id, synonym) select id, 'Administrirovanie' from book_rubric where parent_id is null and name = 'Администрирование';
insert into book_rubric_index(id, synonym) select id, 'Yazyki_programmirovaniya' from book_rubric where parent_id is null and name = 'Языки программирования';
insert into book_rubric_index(id, synonym) select id, 'Mobilnaya_razrabotka' from book_rubric where parent_id is null and name = 'Мобильная разработка';
insert into book_rubric_index(id, synonym) select id, 'Freymvorki_i_formaty' from book_rubric where parent_id is null and name = 'Фреймворки и форматы';
insert into book_rubric_index(id, synonym) select id, 'Bezopasnost' from book_rubric where parent_id is null and name = 'Безопасность';
insert into book_rubric_index(id, synonym) select id, 'Operatsionnye_sistemy' from book_rubric where parent_id is null and name = 'Операционные системы';
insert into book_rubric_index(id, synonym) select id, 'Computer_Science' from book_rubric where parent_id is null and name = 'Computer Science';
insert into book_rubric_index(id, synonym) select id, 'Arkhitektura_i_patterny' from book_rubric where parent_id is null and name = 'Архитектура и паттерны';
insert into book_rubric_index(id, synonym) select id, 'DevOps' from book_rubric where parent_id is null and name = 'DevOps';
insert into book_rubric_index(id, synonym) select id, 'Stat_zvezdoy' from book_rubric where parent_id is null and name = 'Стать звездой';
insert into book_rubric_index(id, synonym) select id, 'DataIntensive_Apps' from book_rubric where parent_id is null and name = 'Data-Intensive Apps';
insert into book_rubric_index(id, synonym) select id, 'Verstka' from book_rubric where parent_id is null and name = 'Верстка';
insert into book_rubric_index(id, synonym) select id, 'Manualy_ot_Intel' from book_rubric where parent_id is null and name = 'Мануалы от Intel';
insert into book_rubric_index(id, synonym) select id, 'Dizayn' from book_rubric where parent_id is null and name = 'Дизайн';
insert into book_rubric_index(id, synonym) select id, 'Illyustratsiya_i_risunok' from book_rubric where parent_id is null and name = 'Иллюстрация и рисунок';
insert into book_rubric_index(id, synonym) select id, 'Infografika' from book_rubric where parent_id is null and name = 'Инфографика';
insert into book_rubric_index(id, synonym) select id, 'Proektirovanie_interfeysov' from book_rubric where parent_id is null and name = 'Проектирование интерфейсов';
insert into book_rubric_index(id, synonym) select id, 'Issledovaniya' from book_rubric where parent_id is null and name = 'Исследования';
insert into book_rubric_index(id, synonym) select id, 'Tipografika' from book_rubric where parent_id is null and name = 'Типографика';
insert into book_rubric_index(id, synonym) select id, 'Gorodskaya_sreda' from book_rubric where parent_id is null and name = 'Городская среда';
insert into book_rubric_index(id, synonym) select id, 'Testirovanie' from book_rubric where parent_id is null and name = 'Тестирование';
insert into book_rubric_index(id, synonym) select id, 'Avtotesty' from book_rubric where parent_id is null and name = 'Автотесты';
insert into book_rubric_index(id, synonym) select id, 'Analitika' from book_rubric where parent_id is null and name = 'Аналитика';
insert into book_rubric_index(id, synonym) select id, 'Psikhologiya_i_samorazvitie' from book_rubric where parent_id is null and name = 'Психология и саморазвитие';
insert into book_rubric_index(id, synonym) select id, 'Effektivnaya_rabota' from book_rubric where parent_id is null and name = 'Эффективная работа';
insert into book_rubric_index(id, synonym) select id, 'Neyronauka_i_evolyutsiya' from book_rubric where parent_id is null and name = 'Нейронаука и эволюция';
insert into book_rubric_index(id, synonym) select id, 'Tekhnologii' from book_rubric where parent_id is null and name = 'Технологии';
insert into book_rubric_index(id, synonym) select id, 'Upravlenie_proektami' from book_rubric where parent_id is null and name = 'Управление проектами';
insert into book_rubric_index(id, synonym) select id, 'Upravlenie_komandami' from book_rubric where parent_id is null and name = 'Управление командами';
insert into book_rubric_index(id, synonym) select id, 'Marketing' from book_rubric where parent_id is null and name = 'Маркетинг';
insert into book_rubric_index(id, synonym) select id, 'Redaktura' from book_rubric where parent_id is null and name = 'Редактура';
insert into book_rubric_index(id, synonym) select id, 'Nauka' from book_rubric where parent_id is null and name = 'Наука';
insert into book_rubric_index(id, synonym) select id, 'Matematika_i_statistika' from book_rubric where parent_id is null and name = 'Математика и статистика';
insert into book_rubric_index(id, synonym) select id, 'Prochee' from book_rubric where parent_id is null and name = 'Прочее';

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Razrabotka' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Bazy_dannykh_i_pamyat',
                 'Administrirovanie',
                 'Yazyki_programmirovaniya',
                 'Mobilnaya_razrabotka',
                 'Freymvorki_i_formaty',
                 'Bezopasnost',
                 'Operatsionnye_sistemy',
                 'Computer_Science',
                 'Arkhitektura_i_patterny',
                 'DevOps',
                 'Stat_zvezdoy',
                 'DataIntensive_Apps',
                 'Verstka',
                 'Manualy_ot_Intel');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Dizayn' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Dizayn', 'Illyustratsiya_i_risunok', 'Infografika', 'Proektirovanie_interfeysov', 'Issledovaniya', 'Tipografika', 'Gorodskaya_sreda');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Testirovanie' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Testirovanie', 'Avtotesty');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Analitika' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Analitika');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Chelovek' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Psikhologiya_i_samorazvitie', 'Effektivnaya_rabota', 'Neyronauka_i_evolyutsiya');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Processy_i_komanda' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Tekhnologii', 'Upravlenie_proektami', 'Upravlenie_komandami');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Marketing' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Marketing', 'Redaktura');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Nauka' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Nauka', 'Matematika_i_statistika');

update book_rubric
set parent_id = (select id from book_rubric_index where "synonym" = 'Parent_Prochee' limit 1)
from book_rubric_index bri
where book_rubric.id = bri.id
  and synonym in('Prochee');

update book_rubric set parent_id = null where parent_id =-1;

insert into librarian(name, contacts, is_deleted)values

('Иван Иванов','[{"Type": "telegram", "Value": "kvsvsdjvbjerjefvwdqwe123"},{"Type": "email", "Value": "fake@email.ru"},{"Type": "phone", "Value": "+7 800 555 35 35"}]', false);