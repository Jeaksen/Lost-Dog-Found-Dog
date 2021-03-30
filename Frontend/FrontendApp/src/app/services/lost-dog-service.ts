import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { LostDogsResponse, PostLostDogResponse, MarkLostDogAsFoundResponse } from '../models/responses';
import { environment } from '../environments/environment-dev';
import { Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LostDogService {
    private url = environment.apiUrl;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

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

    MarkLostDogAsFound(lostDogId: number): Observable<MarkLostDogAsFoundResponse> {
        return this.http.put<MarkLostDogAsFoundResponse>(this.url + `lostdogs/${lostDogId}/found`, '', this.httpOptions)
            .pipe(
                tap(_ => {
                    this.log('marked a lost dog as found');
                    console.log(_);
                }),
                catchError(this.handleError<MarkLostDogAsFoundResponse>('postLostDog', undefined))
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