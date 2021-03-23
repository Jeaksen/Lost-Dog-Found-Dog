import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/responses';
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

    login(name: string, pass: string) {
        console.log(new LoginRequest(name, pass))
        return this.http.post<LoginResponse>(this.url + 'login', new LoginRequest(name, pass))
            .pipe(map(response => {
                localStorage.setItem('authToken', response.data.token);
                console.log(response)
                return response;
            }));
    }

    logout() {
        localStorage.removeItem('authToken');
    }
}