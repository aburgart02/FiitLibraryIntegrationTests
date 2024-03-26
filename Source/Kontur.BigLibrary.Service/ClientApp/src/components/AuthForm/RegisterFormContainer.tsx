import * as React from "react";
import { useHistory } from "react-router-dom";
import { api } from "src/api/Api";
import "./styles.css";
import { RegisterForm } from "./RegisterForm";
import { setJwt } from "src/utils/LocalStorageJWT";

export const RegisterFormContainer: React.FC = () => {
    const history = useHistory();
    return <RegisterForm
        onSubmit={async (email, password) => {
            const jwt = await api.login.register({
                email,
                password,
            });

            setJwt(jwt.token);

            history.push("/");
        }}
        validator={api.login.validate}
    />;
};
