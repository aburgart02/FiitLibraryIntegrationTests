import * as React from "react";
import { GroupedRubrics } from "src/models/GroupedRubrics";
import { api } from "src/api/Api";
import Loader from "@skbkontur/react-ui/Loader";
import Gapped from "@skbkontur/react-ui/Gapped";
import { RubricSummary } from "src/models/RubricSummary";
import RenderLayer from "@skbkontur/react-ui/RenderLayer";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { Link } from "src/components/Link/Link";

const styles = require("./RubricList.less");

export interface RubricListProps extends RouteComponentProps {
    onClose: () => void;
    menu: React.RefObject<HTMLDivElement>;
}

export interface RubricListState {
    groupedRubrics: GroupedRubrics[];
}

class RubricList extends React.Component<RubricListProps, RubricListState> {
    state: RubricListState = {
        groupedRubrics: null,
    };

    componentDidMount(): void {
        this.loadRubrics();
    }

    render(): React.ReactNode {
        return (
            <RenderLayer onClickOutside={this.handleClickOutside}>
                {this.state.groupedRubrics == null
                    ? <div className={styles.listLoader}><Loader active={true} /></div>
                    : (
                        <div className={styles.list}>
                            <div className={styles.rubrics}>
                                {this.state.groupedRubrics?.map(x => this.renderGroupedRubric({ groupedRubrics: x }))}
                            </div>
                            <div className={styles.link}><Link to={"/how-it-works"} color="black" onClick={this.props.onClose}>Как всё устроено</Link></div>
                        </div>
                    )
                }
            </RenderLayer>
        );
    }

    private handleClickOutside = (event: Event): void => {
        if (!this.props.menu.current.contains(event.target as Node)) {
            this.props.onClose();
        }
    };

    private loadRubrics = async (): Promise<void> => {
        const groupedRubrics: GroupedRubrics[] = await api.rubric.select();
        groupedRubrics?.sort((a, b) => a.parentRubric.orderId - b.parentRubric.orderId);
        this.setState({ groupedRubrics });
    };

    private renderGroupedRubric: React.FC<{ groupedRubrics: GroupedRubrics }> = ({ groupedRubrics }) => {
        return (
            <div key={groupedRubrics.parentRubric.id} className={styles.block}>
                <Gapped vertical={true} gap={9}>
                    <span className={styles.parentRubricItem}>{groupedRubrics.parentRubric.name}</span>
                    {groupedRubrics.rubrics.map(x => this.renderRubric({ rubric: x }))}
                </Gapped>
            </div>
        );
    };

    private renderRubric: React.FC<{ rubric: RubricSummary }> = ({ rubric }) => {
        return (
            <Link to={`/rubric/${rubric.synonym}`} onClick={this.props.onClose} color="black" key={rubric.id}>
                <span className={styles.rubricItem}>{rubric.name}</span>
            </Link>
        );
    }
}

export default withRouter(RubricList);
