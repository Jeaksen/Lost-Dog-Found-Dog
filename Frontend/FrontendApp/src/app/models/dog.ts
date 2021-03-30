export class Dog {
    id: number;
    name: string;
    breed: string;
    age: number;
    size: string;
    color: string;
    specialMark: string = "";
    hairLength: string;
    earsType: string;
    tailLength: string;
    behaviors: string[] = [''];

    constructor(id: number, name: string, breed: string, age: number, size: string, color: string, specialMark: string,
        hairLength: string, earsType: string, tailLength: string, behaviors: string[]) {
        this.id = id;
        this.name = name;
        this.breed = breed;
        this.age = age;
        this.size = size;
        this.color = color;
        this.specialMark = specialMark;
        this.hairLength = hairLength;
        this.earsType = earsType;
        this.tailLength = tailLength;
        this.behaviors = behaviors;
    }
}