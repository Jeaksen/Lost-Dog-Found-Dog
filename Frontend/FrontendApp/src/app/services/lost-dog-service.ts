import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { environment } from '../environments/environment-dev';

@Injectable({ providedIn: 'root' })
export class LostDogService {
    private url = environment.apiUrl;

    constructor(private http: HttpClient) {
    }

    getAllLostDogsFor(){
        
    }
}