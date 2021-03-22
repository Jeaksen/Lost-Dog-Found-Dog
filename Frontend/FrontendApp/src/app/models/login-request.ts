export class LoginRequest {
    userName: string;
    password: string;

    constructor(name: string, pass: string) {
        this.userName = name;
        this.password = pass;
    }
}