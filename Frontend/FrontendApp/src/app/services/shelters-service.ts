import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { GetSheltersResponse } from '../models/responses';
import { environment } from '../environments/environment-dev';
import { Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class SheltersService {
    private url = environment.apiUrl;

    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private http: HttpClient) {
    }
    
    private log(message: string) {
        console.log(message);
    }

    getAllShelters(): Observable<GetSheltersResponse> {
        return this.http.get<GetSheltersResponse>(this.url + 'shelters')
            .pipe(
                tap(_ => {
                    this.log('fetched lost dogs');
                    console.log(_);
                }),
                catchError(this.handleError<GetSheltersResponse>('getAllShelters', undefined))
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