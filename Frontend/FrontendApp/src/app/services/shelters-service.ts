import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { GetSheltersResponse, RegisterShelterResponse } from '../models/responses';
import { ShelterResponse } from '../models/responses';
import { AllShelterDogsResponse } from '../models/responses';
import { ShelterDogResponse } from '../models/responses';
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

    registerNewShelter(shelter: FormData): Observable<RegisterShelterResponse> {
        return this.http.post<RegisterShelterResponse>(this.url + `shelters`, shelter)
            .pipe(
                tap(_ => {
                    this.log('posted a shelter dog');
                    console.log(_);
                }),
                catchError(this.handleError<RegisterShelterResponse>('postShelterDog'))
            );
    }

    getAllShelters(): Observable<GetSheltersResponse> {
        return this.http.get<GetSheltersResponse>(this.url + 'shelters')
            .pipe(
                tap(_ => {
                    this.log('fetched shelter dogs');
                    console.log(_);
                }),
                catchError(this.handleError<GetSheltersResponse>('getAllShelters', undefined))
            );
    }

    getShelterByID(shelterID: number): Observable<ShelterResponse> {
        return this.http.get<ShelterResponse>(this.url + 'shelters/' + shelterID)
            .pipe(
                tap(_ => {
                    this.log('fetched shelter');
                    console.log(_);
                }),
                catchError(this.handleError<ShelterResponse>('getShelterByID', undefined))
            );
    }

    getAllShelterDogs(shelterID: number): Observable<AllShelterDogsResponse> {
        return this.http.get<AllShelterDogsResponse>(this.url + `shelters/${shelterID}/dogs`)
            .pipe(
                tap(_ => {
                    this.log('fetched all shelter dogs');
                    console.log(_);
                }),
                catchError(this.handleError<AllShelterDogsResponse>('getAllShelterDogs', undefined))
            );
    }

    getShelterDogByID(shelterID: number, dogID: number): Observable<ShelterDogResponse> {
        return this.http.get<ShelterDogResponse>(this.url + `shelters/${shelterID}/dogs/${dogID}`)
            .pipe(
                tap(_ => {
                    this.log('fetched shelter dog');
                    console.log(_);
                }),
                catchError(this.handleError<ShelterDogResponse>('getShelterDogByID', undefined))
            );
    }

    //-------------- ADD/DELETE --------------
    postShelterDog(shelterID: number, shelterDog: FormData): Observable<ShelterDogResponse> {
        return this.http.post<ShelterDogResponse>(this.url + `shelters/${shelterID}/dogs`, shelterDog)
            .pipe(
                tap(_ => {
                    this.log('posted a shelter dog');
                    console.log(_);
                }),
                catchError(this.handleError<ShelterDogResponse>('postShelterDog'))
            );
    }

    deleteShelterDog(shelterID: number, dogID: number): Observable<ShelterDogResponse> {
        return this.http.delete<ShelterDogResponse>(this.url + `shelters/${shelterID}/dogs/${dogID}`)
            .pipe(
                tap(_ => {
                    this.log('deleted a shelter dog');
                    console.log(_);
                }),
                catchError(this.handleError<ShelterDogResponse>('deleteShelterDog'))
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