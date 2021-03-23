import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { LostDogsResponse, PostLostDogResponse } from '../models/respones';
import { environment } from '../environments/environment-dev';
import { Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LostDogService {
    private url = environment.apiUrl;

    constructor(private http: HttpClient) {
    }
    
    private log(message: string) {
        console.log(message);
    }

    getAllLostDogs(): Observable<LostDogsResponse> {
        return this.http.get<LostDogsResponse>(this.url + 'lostdogs')
            .pipe(
                tap(_ => {
                    this.log('fetched lost dogs');
                    console.log(_);
                }),
                catchError(this.handleError<LostDogsResponse>('getAllLostDogs', undefined))
            );
    }

    postLostDog(lostDog: FormData): Observable<PostLostDogResponse> {
        return this.http.post<PostLostDogResponse>(this.url + 'lostdogs', lostDog)
            .pipe(
                tap(_ => {
                    this.log('posted a lost dog');
                    console.log(_);
                }),
                catchError(this.handleError<PostLostDogResponse>('postLostDog'))
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