import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { UserDetailsResponse } from '../models/responses';
import { environment } from '../environments/environment-dev';
import { Observable, of, throwError } from "rxjs";
import { UserDetailsData } from "../models/data";
import { UpdateUserDataRequest } from "../models/update-user-data-request";
import { AuthenticationService } from "./authentication-service";

@Injectable({ providedIn: 'root' })
export class UserService {
    private url = environment.apiUrl;

    httpOptions = {
        //headers: new HttpHeaders({ 'Content-Type': 'application/json' }) don't set this header never ever I beg you
        //headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': this.authenticationService.token! })
        //headers: new HttpHeaders({ 'Accept': '*/*', 'Authorization': this.authenticationService.token! })
    };

    constructor(private http: HttpClient, private authenticationService: AuthenticationService) {
    }
    
    private log(message: string) {
        console.log(message);
    }

    getUserDetails(userId: number): Observable<UserDetailsResponse> {
        return this.http.get<UserDetailsResponse>(this.url + `user/${userId}`, this.httpOptions)
            .pipe(
                tap(_ => {
                    this.log('fetched user details');
                    console.log(_);
                }),
                catchError(this.handleError<UserDetailsResponse>('getUserDetails', undefined))
            );
    }

    updateUserDetails(editInfoForm: FormData, userId: number): Observable<UserDetailsResponse> {
        //debugger
        //const token = this.authenticationService.token;
        //const userDetails: UpdateUserDataRequest = new UpdateUserDataRequest(userName, email, number);
        // const costam = fetch(this.url + `user/${userId}`, { method: 'PUT', headers: {
        //     Authorization: `${token}`,
        //     Accept: "*/*"
        // }, body: editInfoForm })
        // costam.then(response => response.json()).then(dostajecos => console.log(dostajecos)).catch(
        //     e => console.log(e)
        // )
        return this.http.put<UserDetailsResponse>(this.url + `user/${userId}`, editInfoForm, this.httpOptions)
            .pipe(
                tap(_ => {
                    this.log('fetched user details');
                    console.log(_);
                }),
                catchError(this.handleError<UserDetailsResponse>('getUserDetails', undefined))
            );
    }

    /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

            // TODO: send the error to remote logging infrastructure
            console.error(error); // log to console instead

            // TODO: better job of transforming error for user consumption
            //this.log(`${operation} failed: ${error.message}`);

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}