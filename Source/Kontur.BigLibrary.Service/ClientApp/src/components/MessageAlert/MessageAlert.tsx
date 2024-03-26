import * as React from "react";

export interface MessageAlertProps {
    message: string;
    type: "success" | "info" | "warning" | "danger";
    timeout?: number;
}

const MessageAlert: React.FC<MessageAlertProps> = ({ message, type, timeout = 3000 }) => {
    const [visible, setVisible] = React.useState(false);

    React.useEffect(() => {
        setVisible(!!message);
        const timer = setTimeout(() => {
            setVisible(false);
        }, timeout);

        return () => clearTimeout(timer);
    }, [message, type]);

    return visible ? (
        <div className={`alert alert-${type} position-absolute`} style={{ top: "10px", right: "10px", zIndex: 1000 }} role="alert">
            {message}
        </div>
    ) : null;
};

export default MessageAlert;
