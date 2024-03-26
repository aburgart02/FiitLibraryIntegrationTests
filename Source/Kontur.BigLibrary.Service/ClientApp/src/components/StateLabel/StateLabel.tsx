import * as React from "react";

const classNames = require("classnames");
const styles = require("./StateLabel.less");

export interface StateLabelProps {
    isBusy: boolean;
}

export default function StateLabel(props: StateLabelProps): React.ReactElement {
    const labelColor = props.isBusy ? styles.busyLabel : styles.freeLabel;

    return (
        <div data-tid='state-label' className={classNames(styles.label, labelColor)}>
            {props.isBusy ? "ЗАНЯТА" : "СВОБОДНА"}
        </div>
    );
}
