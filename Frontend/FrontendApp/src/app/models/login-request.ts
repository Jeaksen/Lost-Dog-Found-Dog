export class LoginRequest {
    username: string;
    password: string;

    constructor(name: string, pass: string) {
        this.username = name;
        this.password = pass;
    }
}