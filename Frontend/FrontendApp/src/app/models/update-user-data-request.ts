export class UpdateUserDataRequest {
    userName: string;
    email: string;
    phoneNumber: string;

    constructor(name: string, email: string, phone: string) {
        this.userName = name;
        this.email = email;
        this.phoneNumber = phone;
    }
}