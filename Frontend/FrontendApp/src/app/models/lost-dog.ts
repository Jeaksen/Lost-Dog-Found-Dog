import { Dog } from './dog'
import { Location } from './location'
import { Picture } from './picture'

export class LostDog extends Dog {
    ownerId: number;
    location: Location;
    dateLost: Date;
    picture: Picture;
    isFound: boolean;

    constructor(id: number, name: string, breed: string, age: number, size: string, color: string, specialMark: string,
        hairLength: string, earsType: string, tailLength: string, behaviors: string[], ownerId: number, location: Location,
        dateLost: Date, picture: Picture) 
    {
        super(id, name, breed, age, size, color, specialMark, hairLength, earsType, tailLength, behaviors);
        this.ownerId = ownerId;
        this.location = location;
        this.dateLost = dateLost;
        this.picture = picture;
        this.isFound = false;
    }
}