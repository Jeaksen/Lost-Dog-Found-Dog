import { Address } from './address';

export class Shelter {
    id?: number;
    name: string;
    address: Address;
    phoneNumber: string;
    email: string;

    constructor(name: string, address: Address, phoneNumber: string, email: string) {
        this.name = name;
        this.address = address;
        this.phoneNumber = phoneNumber;
        this.email = email;
    }
}