import { Injectable } from "@angular/core";
import { Subject } from 'rxjs';
import { environment } from '../environments/environment-dev';

@Injectable()
export class HelperService {

  private messageSource = new Subject<number>();
  lostDogID$ = this.messageSource.asObservable();

  constructor() { }

  sendLostDogID(message: number) {
    this.messageSource.next(message);
  }

}