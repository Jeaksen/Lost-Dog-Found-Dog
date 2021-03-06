import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { LostDogsResponse, PostLostDogResponse, MarkLostDogAsFoundResponse, PostCommentResponse, BaseResponse } from '../models/responses';
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

    getFilteredLostDogs(filter: string): Observable<LostDogsResponse> {
        return this.http.get<LostDogsResponse>(this.url + 'lostdogs?' + filter)
            .pipe(
                tap(_ => {
                    this.log('fetched lost dogs');
                    console.log(_);
                }),
                catchError(this.handleError<LostDogsResponse>('getAllLostDogs', undefined))
            );
    }

    getLostDogByID(lostDogId: number): Observable<PostLostDogResponse> {
        return this.http.get<PostLostDogResponse>(this.url + 'lostdogs/' + lostDogId)
            .pipe(
                tap(_ => {
                    this.log('fetched a lost dog');
                    console.log(_);
                }),
                catchError(this.handleError<PostLostDogResponse>('getLostDogByID', undefined))
            );
    }

    getUserLostDogs(userId: string): Observable<LostDogsResponse> {
        return this.http.get<LostDogsResponse>(this.url + `lostdogs?filter.ownerId=${userId}`)
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

    putLostDog(lostDog: FormData, lostDogId: number): Observable<PostLostDogResponse> {
        return this.http.put<PostLostDogResponse>(this.url + 'lostdogs/' + lostDogId, lostDog)
            .pipe(
                tap(_ => {
                    this.log('put a lost dog');
                    console.log(_);
                }),
                catchError(this.handleError<PostLostDogResponse>('putLostDog'))
            );
    }

    markLostDogAsFound(lostDogId: number): Observable<MarkLostDogAsFoundResponse> {
        return this.http.put<MarkLostDogAsFoundResponse>(this.url + `lostdogs/${lostDogId}/found`, '', this.httpOptions)
            .pipe(
                tap(_ => {
                    this.log('marked a lost dog as found');
                    console.log(_);
                }),
                catchError(this.handleError<MarkLostDogAsFoundResponse>('postLostDog', undefined))
            );
    }

    addCommentToLostDog(lostDogComment: FormData, lostDogId: number): Observable<PostCommentResponse> {
        return this.http.post<PostCommentResponse>(`${this.url}lostdogs/${lostDogId}/comments`, lostDogComment)
            .pipe(
                tap(_ => {
                    this.log('posted a comment');
                    console.log(_);
                }),
                catchError(this.handleError<PostCommentResponse>('postComment'))
            );
    }

    deleteComment(lostDogId: number, commentId: number): Observable<BaseResponse> {
        return this.http.delete<BaseResponse>(`${this.url}lostdogs/${lostDogId}/comments/${commentId}`)
            .pipe(
                tap(_ => {
                    this.log('deleted a comment');
                    console.log(_);
                }),
                catchError(this.handleError<BaseResponse>('deleteComment'))
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