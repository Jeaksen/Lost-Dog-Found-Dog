export class Address {
    city: string;
    street: string;
    postCode: string;
    buildingNumber: string;
    additionalAddressLine?: string;

    constructor(city: string, street: string, postCode: string, buildingNumber: string, additionalAddressLine: string = '') {
        this.city = city;
        this.street = street;
        this.postCode = postCode;
        this.buildingNumber = buildingNumber;

        if (additionalAddressLine !== '') {
            this.additionalAddressLine = additionalAddressLine;
        }
    }
}