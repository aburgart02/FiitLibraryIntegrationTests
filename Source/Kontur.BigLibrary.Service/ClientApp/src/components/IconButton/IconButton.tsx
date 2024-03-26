import Icon, { IconName } from "@skbkontur/react-icons";
import * as React from "react";

const classNames = require("classnames/bind");
const styles = require("./IconButton.less");
const cn = classNames.bind(styles);

interface Props {
    icon: IconName;
    onClick?: () => void;
    dataTid: string;
}

export const IconButton = (props: Props) => (
    <button data-tid={props.dataTid} className={cn(styles.iconButton)} onClick={props.onClick}>
        <Icon name={props.icon} size={24} />
    </button>
);
