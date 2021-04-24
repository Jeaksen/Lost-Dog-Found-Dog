import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { LoginRequest } from '../models/login-request';
import { RegisterUserRequest } from '../models/register-user-request';
import { LoginResponse, RegisterUserResponse } from '../models/responses';
import { environment } from '../environments/environment-dev';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private url = environment.apiUrl;

    public get loggedIn(): boolean {
        return localStorage.getItem('authToken') !== null;
    }

    public get token(): string | null {
        return localStorage.getItem('authToken');
    }

    constructor(private http: HttpClient) {
    }

    login(loginData: FormData) {
        return this.http.post<LoginResponse>(this.url + 'login', loginData)
            .pipe(map(response => {
                localStorage.setItem('authToken', response.data.token);
                localStorage.setItem('userId', response.data.id.toString());
                console.log(response)
                return response;
            }));
    }

    logout() {
        localStorage.removeItem('authToken');
    }

    register(registerData: FormData) {
        //console.log(request)
        return this.http.post<RegisterUserResponse>(this.url + 'register', registerData)
            .pipe(map(response => {
                console.log(response)
                return response;
            }));
    }
}