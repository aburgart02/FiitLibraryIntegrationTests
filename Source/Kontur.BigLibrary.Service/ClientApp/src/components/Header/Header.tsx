import * as React from "react";
import SearchIcon from "@skbkontur/react-icons/Search";
import Sticky from "@skbkontur/react-ui/Sticky";
import RubricList from "src/components/RubricList/RubricList";
import MenuIcon from "@skbkontur/react-icons/Menu";
import Autocomplete from "@skbkontur/react-ui/Autocomplete";
import { Link, RouteComponentProps, withRouter } from "react-router-dom";
import { User } from "src/models/User";
import { api } from "src/api/Api";
import { CurrentUser } from "src/components/CurrentUser/CurrentUser";
import { IconButton } from "../IconButton";
import { exportBooks } from "./helper";

const style = require("./Header.less");

export interface HeaderState {
    isRubricListOpen: boolean;
    query: string;
    user: User;
}

class Header extends React.Component<RouteComponentProps<{ query: string }>, HeaderState> {
    private menu: React.RefObject<HTMLDivElement> = React.createRef();

    state: HeaderState = {
        isRubricListOpen: false,
        query: this.props.match.params.query || "",
        user: null,
    };

    async componentDidMount(): Promise<void> {
        await this.loadUser();
    }

    componentDidUpdate(prevProps: Readonly<RouteComponentProps<{ query: string }>>): void {
        if (prevProps.match.params.query !== this.props.match.params.query)
            this.setState({ query: this.props.match.params.query || "" });
    }

    render(): React.ReactNode {
        return (
            <Sticky side={"top"}>
                <div className={style.header}>
                    <div className={style.headerMenu}>
                        <div ref={this.menu} onClick={this.handleRubricMenuClick} className={style.menu}>
                            <MenuIcon size={35} />
                        </div>
                        <Link to={"/"} className={style.title}>Библиотека</Link>
                        <IconButton  icon={"DocumentTypeXml"} onClick={exportBooks} dataTid="xml-download"
                        />
                        <div data-tid='search-input' style={{ marginTop: "10px" }}>
                            <Autocomplete
                                width={"500px"}
                                size={"large"}
                                placeholder={"найти по названию, автору или рубрике"}
                                borderless
                                leftIcon={<div style={{ marginTop: "-4px" }}><SearchIcon size={18} /></div>}
                                onChange={(_, v) => this.setState({ query: v })}
                                value={decodeURIComponent(this.state.query)}
                                onKeyDown={(event) => this.search(event)}
                            />
                        </div>
                    </div>
                    <CurrentUser user={this.state.user} />
                </div>
                <div className={style.border} />
                {this.state.isRubricListOpen && <RubricList menu={this.menu} onClose={() => this.setState({ isRubricListOpen: false })} />}
            </Sticky>
        );
    }

    private handleRubricMenuClick = (): void => {
        this.setState((state) => {
            return { isRubricListOpen: !state.isRubricListOpen };
        });
    };

    private search = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === "Enter") {
            const queryForSearch = encodeURIComponent(this.state.query);
            queryForSearch
                ? this.props.history.replace(`/query/${queryForSearch}`)
                : this.props.history.replace("/");
        }
    };

    loadUser = async () => {
        const currentUser = await api.user.get();
        this.setState({ user: currentUser });
    };
}

export default withRouter(Header);
