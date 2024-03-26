import * as React from "react";
import {RouteComponentProps, withRouter} from "react-router";
import {NotFoundError, ServerError} from "src/api/Errors";

class GlobalErrorHandlerInternal extends React.Component<RouteComponentProps> {
    componentDidMount(): void {
        window.onunhandledrejection = (error: PromiseRejectionEvent) => this.handleError(error.reason);
    }

    render(): React.ReactNode {
        return null;
    }

    private handleError(error: Error): void {
        if (error instanceof NotFoundError) {
            this.props.history.push("/not-found");
        }
        else if (error instanceof ServerError) {
            this.props.history.push("/server-error");
        }
    }
}

export const GlobalErrorHandler = withRouter(GlobalErrorHandlerInternal);
