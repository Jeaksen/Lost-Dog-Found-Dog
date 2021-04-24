export class RegisterUserRequest {
    username: string;
    password: string;
    email: string;
    phone_number: string;

    constructor(name: string, pass: string, email: string, phone: string) {
        this.username = name;
        this.password = pass;
        this.email = email;
        this.phone_number = phone;
    }
}