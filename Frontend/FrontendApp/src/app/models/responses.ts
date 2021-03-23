import { LoginData } from './data';
import { LostDog } from './lost-dog';

interface BaseResponse {
    statusCode: number;
    successful: boolean;
    message: string;
}

export interface LoginResponse extends BaseResponse {
    data: LoginData;
}

export interface LostDogsResponse extends BaseResponse {
    data: LostDog[];
}

export interface PostLostDogResponse extends BaseResponse {
    data: LostDog;
}