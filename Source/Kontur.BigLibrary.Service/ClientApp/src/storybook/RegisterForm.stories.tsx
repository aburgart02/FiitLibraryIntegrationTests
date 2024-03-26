import * as React from "react";
import type { Meta, StoryObj } from '@storybook/react';
import { RegisterForm } from "src/components/AuthForm/RegisterForm";
import { PasswordValidationResult } from "src/models/UserLogin";
import 'bootstrap/dist/css/bootstrap.min.css';

const meta: Meta<typeof RegisterForm> = {
    title: "RegisterForm",
    component: RegisterForm,
};

export default meta;
type Story = StoryObj<typeof RegisterForm>;

export const Primary: Story = {
    render: () => <RegisterForm onSubmit={console.info} validator={(password: string): Promise<PasswordValidationResult> => new Promise((resolve) => {
        console.info(password)
        const result = { isValid: true, strength: "Strong" } as PasswordValidationResult;
        resolve(result);
    })} />,
};
