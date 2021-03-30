import { LoginData, RegisterUserData } from './data';
import { LostDog } from './lost-dog';

interface BaseResponse {
    statusCode: number;
    successful: boolean;
    message: string;
}

export interface LoginResponse extends BaseResponse {
    data: LoginData;
}

export interface RegisterUserResponse extends BaseResponse {
    data: RegisterUserData;
}

export interface LostDogsResponse extends BaseResponse {
    data: LostDog[];
}

export interface PostLostDogResponse extends BaseResponse {
    data: LostDog;
}

export interface MarkLostDogAsFoundResponse extends BaseResponse {
    data: boolean;
}