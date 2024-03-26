export interface UserLoginModel {
    email: string;
    password: string;
}

export interface JWTModel {
    token: string;
}

export interface PasswordValidationResult {
    isValid: boolean;
    strength: "Weak" | "Good" | "Strong";
}
