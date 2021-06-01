import { Location } from "./location";
import { Picture } from "./picture";

export interface LoginData {
    userType: string;
    id: number;
    token: string;
}

export interface RegisterUserData {
}

export interface UserDetailsData {
    id: number;
    name: string;
    email: string;
    phoneNumber: string;
}

export interface CommentData {
    id: number;
    dogId: number;
    author: UserDetailsData;
    text: string;
    location: Location;
    picture: Picture;
}