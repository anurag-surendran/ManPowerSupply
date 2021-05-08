import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private baseUrl: string = `${environment.apiUrl}Dashboard/`;

  constructor(private _http: HttpClient, public datepipe: DatePipe) {

  }

  getCustomerBalance() {
    let url = `${this.baseUrl}GetCustomerBalance`;
    return this._http.get(url).pipe();
  }

  getEmployeeBalance() {
    let url = `${this.baseUrl}GetEmployeeBalance`;
    return this._http.get(url).pipe();
  }

  getCurrentAmount() {
    let url = `${this.baseUrl}GetCurrentAmount`;
    return this._http.get(url).pipe();
  }

  getConsolidatedAttendance(isCurrentMonth:Boolean) {
    let url = `${this.baseUrl}GetConsolidatedAttendance/${isCurrentMonth}`;
    return this._http.get(url).pipe();
  }
}
