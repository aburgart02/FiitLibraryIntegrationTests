export class NotFoundError extends Error {
    constructor(errorMessage?: string) {
        super(errorMessage || "Not Found");
        Object.setPrototypeOf(this, NotFoundError.prototype);
    }
}

export class ServerError extends Error {
    constructor(errorMessage?: string) {
        super(errorMessage || "Server Error");
        Object.setPrototypeOf(this, ServerError.prototype);
    }
}

export class UnauthorizedError extends Error {
    constructor(errorMessage?: string) {
        super(errorMessage || "Unauthorized");
        Object.setPrototypeOf(this, UnauthorizedError.prototype);
    }
}
