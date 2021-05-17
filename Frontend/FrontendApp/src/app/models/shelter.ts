import { Address } from './address';

export interface Shelter {
    id: number;
    name: string;
    address: Address;
    phoneNumber: string;
    email: string;
}