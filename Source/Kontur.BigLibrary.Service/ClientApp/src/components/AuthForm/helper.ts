import { PasswordValidationResult } from "src/models/UserLogin";

export const validateFields = (email: string, password: string, setEmailError: (value: string) => void, setPassError: (value: string) => void) => {
    const cleanErrors = () => {
        setEmailError(null);
        setPassError(null);
    };

    cleanErrors();

    if (!email) {
        setEmailError("Поле электронной почты не должно быть пустым.");
        return false;
    }

    const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    if (!emailRegex.test(email)) {
        setEmailError("Пожалуйста, введите корректный адрес электронной почты.");
        return false;
    }

    if (!password) {
        setPassError("Пароль не должен быть пустым.");
        return false;
    }

    if (password.length < 6 || password.length > 15) {
        setPassError("Пароль должен быть от 6 до 15 символов.");
        return false;
    }

    return true;
};

export const isPasswordValid = async (
    password: string,
    setPasswordStrength: (value: string) => void,
    validator: (password: string) => Promise<PasswordValidationResult>
): Promise<boolean> => {
    if (password.length < 6 || password.length > 15) {
        setPasswordStrength("weak");
        return false;
    }

    const hasLowerCase = /[a-zа-я]/.test(password);
    const hasUpperCase = /[A-ZА-Я]/.test(password);
    const hasDigits = /[0-9]/.test(password);
    const hasSpecialChars = /[!@#$%^&*]/.test(password);

    if (hasLowerCase && hasUpperCase && hasDigits && hasSpecialChars) {
        if (password.length <= 10) {
            setPasswordStrength("good");
        } else if (password.length > 10) {
            setPasswordStrength("strong");
        }
    } else {
        setPasswordStrength("weak");
    }

    return hasLowerCase && hasUpperCase && hasDigits && hasSpecialChars && (await validator(password)).isValid;
};