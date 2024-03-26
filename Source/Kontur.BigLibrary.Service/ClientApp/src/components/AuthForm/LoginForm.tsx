import * as React from "react";
import { useHistory, Link } from "react-router-dom";
import { api } from "src/api/Api";
import "./styles.css";
import { validateFields } from "./helper";
import { InputField, RememberMeCheckbox } from "./Fields";
import { setJwt } from "src/utils/LocalStorageJWT";


export const LoginForm: React.FC = () => {
    const history = useHistory();
    const [email, setEmail] = React.useState("");
    const [password, setPassword] = React.useState("");
    const [rememberMe, setRememberMe] = React.useState(false);
    const [emailError, setEmailError] = React.useState<string | null>(null);
    const [passError, setPassError] = React.useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!validateFields(email, password, setEmailError, setPassError)) {
            return;
        }

        try {
            const jwt = await api.login.login({
                email,
                password
            });

            setJwt(jwt.token);

            history.push("../");
        } catch (error) {
            const errorMessage = error.message;
            const errorData = JSON.parse(errorMessage);

            if (errorData["Email"]) {
                setEmailError(errorData["Email"][0]);
            } else {
                setPassError(errorData["Password"][0]);
            }
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100">
            <div className="col-md-4">
                <form onSubmit={handleSubmit} noValidate>
                    <h4>Вход</h4>
                    <hr />
                    <InputField data_tid="email" label="Электронная почта" type="email" value={email} onChange={setEmail} error={emailError} />
                    <InputField data_tid="password" label="Пароль" type="password" value={password} onChange={setPassword} error={passError} />
                    <RememberMeCheckbox checked={rememberMe} onChange={setRememberMe} />
                    <div className="form-group">
                        <button type="submit" className="btn btn-primary">
                            Войти
                        </button>
                    </div>
                    <div className="form-group">
                        <p>
                            <Link to="/register">Регистрация</Link>
                        </p>
                    </div>
                </form>
            </div>
        </div>
    );
};
