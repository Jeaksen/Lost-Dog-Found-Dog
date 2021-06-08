import { CommentData, LoginData, RegisterUserData, UserDetailsData } from './data';
import { LostDogFromBackend } from './lost-dog-from-backend';
import { Shelter } from './shelter';
import { ShelterDog } from './shelter-dog';

export interface BaseResponse {
    statusCode: number;
    successful: boolean;
    message: string;
    metadata: number;
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

export interface ShelterResponse extends BaseResponse {
    data: Shelter;
}

export interface GetSheltersResponse extends BaseResponse {
    data: Shelter[];
}

export interface ShelterDogResponse extends BaseResponse {
    data: ShelterDog;
}

export interface AllShelterDogsResponse extends BaseResponse {
    data: ShelterDog[];
}

export interface PostCommentResponse extends BaseResponse {
    data: CommentData;
}

export interface RegisterShelterResponse extends BaseResponse {
    data: Shelter;
}

// export interface RegisterShelterResponse extends BaseResponse {
//     data: Shelter | null;
// }

// export interface RegisterShelterResponse extends BaseResponse {
//     data: Shelter | null;
//     successful: boolean;
//     message: string;
// }