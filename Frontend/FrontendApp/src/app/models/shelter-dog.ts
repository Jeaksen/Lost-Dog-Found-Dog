import { Dog } from './dog'
import { Picture } from './picture'

export class ShelterDog extends Dog {
    shelterId?: string;
    picture?: Picture;

    constructor(name: string, breed: string, age: number, size: string, color: string, specialMark: string,
        hairLength: string, earsType: string, tailLength: string, behaviors: string[]) 
    {
        super(name, breed, age, size, color, specialMark, hairLength, earsType, tailLength, behaviors);
    }
}