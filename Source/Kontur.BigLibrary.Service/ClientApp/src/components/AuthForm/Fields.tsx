import * as React from "react";

export interface InputFieldProps {
    data_tid : string;
    label: string;
    type: string;
    value: string;
    onChange: (value: string) => void;
    error: string | null;
}

export const InputField: React.FC<InputFieldProps> = ({ data_tid, label, type, value, onChange, error, }) => (
    <div data-tid={data_tid} className="form-group">
        <label>{label}</label>
        <input
            data-tid='input'
            type={type}
            id={data_tid}
            className={`form-control ${error ? "is-invalid" : ""}`}
            value={value}
            onChange={(e) => onChange(e.target.value)}
            required
        />
        <div data-tid='validation-message' className="invalid-feedback">{error}</div>
    </div>
);

export const RememberMeCheckbox: React.FC<{ checked: boolean, onChange: (value: boolean) => void }> = ({ checked, onChange }) => (
    <div className="form-group form-check">
        <input
            type="checkbox"
            className="form-check-input"
            id="rememberMe"
            checked={checked}
            onChange={() => onChange(!checked)}
        />
        <label className="form-check-label" htmlFor="rememberMe">
            Запомнить меня?
        </label>
    </div>
);