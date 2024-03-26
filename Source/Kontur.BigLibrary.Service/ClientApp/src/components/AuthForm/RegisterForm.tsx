import * as React from "react";
import { Link } from "react-router-dom";
import { PasswordValidationResult } from "src/models/UserLogin";
import "./styles.css";
import { InputField } from "./Fields";
import { isPasswordValid } from "./helper";

interface Props {
    onSubmit: (email: string, password: string) => void;
    validator: (password: string) => Promise<PasswordValidationResult>;
}

export const RegisterForm: React.FC<Props> = ({ onSubmit, validator }) => {
    const [email, setEmail] = React.useState("");
    const [password, setPassword] = React.useState("");
    const [confirmPassword, setConfirmPassword] = React.useState("");
    const [passwordError, setPasswordError] = React.useState(null);
    const [passwordConfirmError, setPasswordConfirmError] = React.useState(null);
    const [passwordStrength, setPasswordStrength] = React.useState("default");
    const [emailError, setEmailError] = React.useState(null);

    const resetErrors = (): void => {
        setEmailError(null);
        setPasswordError(null);
        setPasswordConfirmError(null);
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault();

     

        if (!email) {
            setEmailError("Поле электронной почты не должно быть пустым.");
            return;
        }
        const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

        if (!emailRegex.test(email)) {
            setTimeout(function() {
                setEmailError("Пожалуйста, введите корректный адрес электронной почты.");
                return;
            },500);
           
        }

        resetErrors();

        if (!(await isPasswordValid(password, setPasswordStrength, validator))) {
            setPasswordError("Пароль не соответствует требованиям.");
            return;
        }

        resetErrors();


        if (password !== confirmPassword) {
            setPasswordConfirmError("Пароли не совпадают.");
            return;
        }

        resetErrors();

        try {
            await onSubmit(email, password);
        } catch (error) {
            const errorMessage = error.message;
            const errorData = JSON.parse(errorMessage);
            const emailError = errorData["Email"][0];

            setEmailError(emailError);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100">
            <div className="col-md-4">
                <form onSubmit={handleSubmit} noValidate>
                    <h4>Регистрация</h4>
                    <hr />
                    <InputField
                        data_tid="email-input"
                        label="Электронная почта"
                        type="email"
                        value={email}
                        onChange={setEmail}
                        error={emailError}
                    />
                    <div  data-tid="password-first" className="form-group">
                        <label>
                            Пароль{" "}
                            <span className={`password-strength-indicator ${passwordStrength}`}>
                                !
                            </span>
                        </label>
                        <input
                            data-tid="input"
                            type="password"
                            id="password"
                            onChange={async (e) => {
                                setPassword(e.target.value);
                                await isPasswordValid(e.target.value, setPasswordStrength, validator);
                            }}
                            className={`form-control ${passwordError !== null ? "is-invalid" : ""}`}
                            value={password}
                            required
                        />
                        <div data-tid='validation-message' className="invalid-feedback">
                            {passwordError}
                        </div>
                        <label>
                            {"Минимальная длина 6 символов, максимальная 15. Обязательно наличие спецсимволов, строчных и прописных букв, цифр."}
                        </label>
                    </div>
                    <InputField
                        data_tid='password-confirmation'
                        label="Подтверждение пароля"
                        type="password"
                        value={confirmPassword}
                        onChange={setConfirmPassword}
                        error={passwordConfirmError}
                    />
                    <div className="form-group">
                        <button type="submit" className="btn btn-primary">
                            Зарегистрироваться
                        </button>
                    </div>
                    <div className="form-group">
                        <p>
                            <Link to="/login">Войти</Link>
                        </p>
                    </div>
                </form>
            </div>
        </div>
    );
};




