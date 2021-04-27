import { Dog } from './dog'
import { Location } from './location'
import { Picture } from './picture'

export class LostDog extends Dog {
    ownerId?: string;
    location: Location;
    dateLost: string;
    picture?: Picture;
    isFound: boolean;

    constructor(name: string, breed: string, age: number, size: string, color: string, specialMark: string,
        hairLength: string, earsType: string, tailLength: string, behaviors: string[], location: Location, dateLost: string) 
    {
        super(name, breed, age, size, color, specialMark, hairLength, earsType, tailLength, behaviors);
        this.location = location;
        this.dateLost = dateLost;
        this.isFound = false;
    }
}