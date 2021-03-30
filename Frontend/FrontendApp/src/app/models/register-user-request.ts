export class RegisterUserRequest {
    userName: string;
    password: string;
    email: string;
    phoneNumber: string;

    constructor(name: string, pass: string, email: string, phone: string) {
        this.userName = name;
        this.password = pass;
        this.email = email;
        this.phoneNumber = phone;
    }
}