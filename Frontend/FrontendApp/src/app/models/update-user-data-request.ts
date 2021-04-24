export class UpdateUserDataRequest {
    name: string;
    phoneNumber: string;
    email: string;

    constructor(name: string, phone: string, email: string) {
        this.name = name;
        this.email = email;
        this.phoneNumber = phone;
    }
}