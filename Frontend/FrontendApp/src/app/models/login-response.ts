import { BaseResponse } from './base-response'

interface data {
    userType: string;
    token: string;
}

export interface LoginResponse extends BaseResponse {
    data: data;
}