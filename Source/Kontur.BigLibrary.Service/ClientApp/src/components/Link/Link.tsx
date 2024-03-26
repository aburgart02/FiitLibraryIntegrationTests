import * as React from "react";
import { Link as RouterLink } from "react-router-dom";

const classNames = require("classnames/bind");
const styles = require("./Link.less");
const cn = classNames.bind(styles);

export interface CustomLinkProps {
    to: string;
    onClick?: () => void;
    color?: "gray" | "black";
    style?: React.CSSProperties;
}

export class Link extends React.Component<CustomLinkProps> {
    render(): React.ReactNode {
        const className = cn("link", this.props.color);
        return (
            <RouterLink to={this.props.to} className={className} onClick={this.props.onClick} style={this.props.style}>
                {this.props.children}
            </RouterLink>
        );
    }
}