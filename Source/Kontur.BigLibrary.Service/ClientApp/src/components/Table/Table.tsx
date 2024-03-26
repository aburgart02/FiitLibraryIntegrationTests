import * as React from "react";

export const Table: React.FC & {
    Thead: React.FC;
    Tbody: React.FC;
    Tr: React.FC;
    Th: React.FC;
    Td: React.FC;
} = (props) => (
    <table className="table">{props.children}</table>
);

Table.Thead = (props) => <thead className="thead-dark">{props.children}</thead>;
Table.Tbody = (props) => <tbody>{props.children}</tbody>;
Table.Tr = (props) => <tr>{props.children}</tr>;
Table.Th = (props) => <th>{props.children}</th>;
Table.Td = (props) => <td>{props.children}</td>;
