import { LoginData, RegisterUserData, UserDetailsData } from './data';
import { LostDog } from './lost-dog';
import { LostDogFromBackend } from './lost-dog-from-backend';

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
    data: LostDogFromBackend[];
}

export interface PostLostDogResponse extends BaseResponse {
    data: LostDogFromBackend;
}

export interface MarkLostDogAsFoundResponse extends BaseResponse {
    data: boolean;
}

export interface UserDetailsResponse extends BaseResponse {
    data: UserDetailsData;
}