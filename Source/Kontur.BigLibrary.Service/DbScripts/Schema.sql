-- Данный скрипт уже применен к biglibrary.db
-- Поменял скрипт - пересоздай БД

create table if not exists book
(
    id int primary key,
    name text not null,
    author text not null,
    description text,
    rubric_id integer not null,
    image_id integer not null,
    creation_datetime timestamp not null default CURRENT_TIMESTAMP,
    is_deleted bool not null,
    count int not null,
    price text null
);

create table if not exists book_rubric
(
    id integer primary key autoincrement,
    name text not null,
    parent_id integer,
    is_deleted bool not null,
    order_id integer not null unique
);

create table if not exists book_image
(
    id int primary key,
    data bytea not null,
    is_deleted bool not null
);


create virtual table if not exists book_index USING fts5
(
    id UNINDEXED,
    fts_lexems,
    synonym
);

-- TODO
-- create virtual table if not exists book_index USING fts5
-- (
--     id int primary key not null,
--     fts_lexems tsvector not null,
--     synonym text not null
-- );

-- create unique index if not exists idx_book_index_unique_synonym
--     on book_index(lower(synonym));

-- create index if not exists idx_gin_book_index_fts_lexems
--     on book_index using gin (fts_lexems);

create table if not exists book_rubric_index
(
    id int primary key not null,
    synonym text not null
);

create unique index if not exists idx_book_rubric_index_unique_synonym
    on book_rubric_index(lower(synonym));

create table if not exists librarian
(
    id integer primary key autoincrement,
    name text not null,
    contacts jsonb not null,
    is_deleted bool not null
);

create table if not exists book_reader
(
    id int primary key,
    book_id int not null,
    user_name text not null,
    start_date timestamp not null,
    is_deleted bool not null
);

create index if not exists idx_book_reader_book_id
    on book_reader(book_id);

create table if not exists book_reader_in_queue
(
    id int primary key,
    book_id int not null,
    user_name text not null,
    start_date timestamp not null,
    is_deleted bool not null
);

create index if not exists idx_book_reader_in_queue_book_id
    on book_reader_in_queue(book_id);

create table if not exists changed_event
(
    sequence_number bigserial,
    timestamp timestamp not null,
    source_type text not null,
    source_id int not null,
    source jsonb,
    constraint pk_changed_event primary key(sequence_number)
);

CREATE TABLE "AspNetRoles" (
   "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
   "Name" TEXT NULL,
   "NormalizedName" TEXT NULL,
   "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "AspNetUsers" (
   "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
   "UserName" TEXT NULL,
   "NormalizedUserName" TEXT NULL,
   "Email" TEXT NULL,
   "NormalizedEmail" TEXT NULL,
   "EmailConfirmed" INTEGER NOT NULL,
   "PasswordHash" TEXT NULL,
   "SecurityStamp" TEXT NULL,
   "ConcurrencyStamp" TEXT NULL,
   "PhoneNumber" TEXT NULL,
   "PhoneNumberConfirmed" INTEGER NOT NULL,
   "TwoFactorEnabled" INTEGER NOT NULL,
   "LockoutEnd" TEXT NULL,
   "LockoutEnabled" INTEGER NOT NULL,
   "AccessFailedCount" INTEGER NOT NULL
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
   "UserId" TEXT NOT NULL,
   "RoleId" TEXT NOT NULL,
   CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
   CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
   CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");