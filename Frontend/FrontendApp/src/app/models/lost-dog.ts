import { Dog } from './dog'
import { Location } from './location'

export class LostDog extends Dog {
    ownerId: number;
    location: Location;
    dateLost: Date;
    isFound: boolean;

    constructor(name: string, breed: string, age: number, size: string, color: string, specialMark: string, hairLength: string,
        earsType: string, tailLength: string, behaviors: string[], ownerId: number, location: Location, dateLost: Date) 
    {
        super(name, breed, age, size, color, specialMark, hairLength, earsType, tailLength, behaviors);
        this.ownerId = ownerId;
        this.location = location;
        this.dateLost = dateLost;
        this.isFound = false;
    }
}