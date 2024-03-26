import * as React from "react";
import LinkBackToAllBooks from "src/components/LinkBackToAllBooks/LinkBackToAllBooks";
import { api } from "src/api/Api";
import { RubricSummary } from "src/models/RubricSummary";
import Toggle from "@skbkontur/react-ui/Toggle";
import Gapped from "@skbkontur/react-ui/Gapped";

const styles = require("./ContentHeader.less");

export interface ContentHeaderProps {
    showOnlyFree: boolean;
    onShowFreeClick: () => void;
    bigText?: string;
    smallText?: string;
    rubricSynonym?: string;
    additionalContent?: React.ReactNode;
}

export interface ContentHeaderState {
    loading: boolean;
    rubric?: RubricSummary;
}

export default class ContentHeader extends React.Component<ContentHeaderProps, ContentHeaderState> {
    state: ContentHeaderState = {
        loading: true,
        rubric: null,
    };

    componentDidMount(): void {
        this.loadData();
    }

    componentDidUpdate(prevProps: ContentHeaderProps): void {
        if (prevProps.rubricSynonym !== this.props.rubricSynonym)
            this.loadData();
    }

    render(): React.ReactNode {
        return (
            <>
                {this.props.bigText && this.renderBigTextContentHeader()}
                {this.props.smallText && this.renderSmallTextContentHeader()}
                {!this.props.smallText && !this.props.bigText && this.renderRubricNameContentHeader()}
            </>
        );
    }

    private renderBigTextContentHeader = (): React.ReactNode => {
        return (
            <ContentHeaderContainer showOnlyFree={this.props.showOnlyFree} onShowFreeClick={this.props.onShowFreeClick}>
                <Gapped gap={16}>
                    <span className={styles.rubricName}>{this.props.bigText}</span>
                    {this.props?.additionalContent}
                </Gapped>
            </ContentHeaderContainer>
        );
    };

    private renderSmallTextContentHeader = (): React.ReactNode => {
        return (
            <ContentHeaderContainer showOnlyFree={this.props.showOnlyFree} onShowFreeClick={this.props.onShowFreeClick}>
                <Gapped gap={16}>
                    <div className={styles.text}>{this.props.smallText}</div>
                    {this.props?.additionalContent}
                </Gapped>
            </ContentHeaderContainer>
        );
    };

    private renderRubricNameContentHeader = (): React.ReactNode => {
        if (this.state.loading) return null;
        return (
            <>
                <div className={styles.link}><LinkBackToAllBooks /></div>
                <ContentHeaderContainer showOnlyFree={this.props.showOnlyFree}
                    onShowFreeClick={this.props.onShowFreeClick}>
                    <Gapped gap={16}>
                        <span className={styles.rubricName}>{this.state.rubric.name}</span>
                        {this.props?.additionalContent}
                    </Gapped>
                </ContentHeaderContainer>
            </>
        );
    };

    private loadData = (): void => {
        this.setState({ loading: true }, async () => {
            if (!this.props.rubricSynonym) {
                return;
            }
            const rubric = await api.rubric.get(this.props.rubricSynonym);
            this.setState({ loading: false, rubric });
        });
    };
}


interface ContentHeaderContainerProps {
    showOnlyFree: boolean;
    onShowFreeClick: () => void;
}

class ContentHeaderContainer extends React.Component<ContentHeaderContainerProps> {
    render(): React.ReactElement {
        return (
            <div className={styles.rubricContainer}>
                {this.props.children}
                <Gapped gap={10}>
                    <Toggle checked={this.props.showOnlyFree} onChange={() => this.props.onShowFreeClick()} />
                    <span>Только свободные</span>
                </Gapped>
            </div>
        );
    }
}