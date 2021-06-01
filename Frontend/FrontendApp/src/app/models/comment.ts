import { Location } from "./location";

export class Comment {
    text: string;
    location: Location;

    constructor(text: string, location: Location) {
        this.text = text;
        this.location = location;
    }
}