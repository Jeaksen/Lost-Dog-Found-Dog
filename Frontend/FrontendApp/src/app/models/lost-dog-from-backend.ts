import { Location } from './location'
import { Picture } from 'src/app/models/picture';
import { CommentData } from './data';

export interface LostDogFromBackend {
    id?: number;
    name: string;
    breed: string;
    age: number;
    size: string;
    color: string;
    specialMark: string;
    hairLength: string;
    earsType: string;
    tailLength: string;
    behaviors: string[];
    ownerId?: number;
    location: Location;
    dateLost: string;
    picture?: Picture;
    isFound: boolean;
    comments: CommentData[];
}