import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessagingService {

  private headerRouteMessageSource = new BehaviorSubject('');
  headerRouteMessage = this.headerRouteMessageSource.asObservable();

  private printTemplateSource = new BehaviorSubject('');
  printTemplate = this.printTemplateSource.asObservable();

  constructor() { }

  changeHeaderRouteMessage(headerRouteData : string){
    this.headerRouteMessageSource.next(headerRouteData);
  } 

  changePrintTemplate(template : string){
    this.printTemplateSource.next(template);
  }
}
