import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UpdatePasswordRequestModel } from '../models/update-password-request-model';
import { UserRequestModel } from '../models/user-request-model';
import { ValidatePasswordRequestModel } from '../models/validate-password-request-model';

@Injectable({
  providedIn: 'root'
})
export class UserManagerService {

  private baseUrl: string = `${environment.apiUrl}UserManager/`;

  constructor(private _http: HttpClient, public datepipe: DatePipe) {

  }

  getAllOrganization() {
    let url = `${this.baseUrl}GetAllOrganization`;
    return this._http.get(url).pipe();
  }

  getAllRoles() {
    let url = `${this.baseUrl}GetAllRoles`;
    return this._http.get(url).pipe();
  }

  addUser(requestModel:UserRequestModel) {
    let url = `${this.baseUrl}AddUser`;
    return this._http.post(url,requestModel).pipe();
  }

  updateUser(requestModel:UserRequestModel,userId:number) {
    let url = `${this.baseUrl}UpdateUser/${userId}`;
    return this._http.put(url,requestModel).pipe();
  }

  validatePassword(requestModel:ValidatePasswordRequestModel) {
    let url = `${this.baseUrl}ValidatePassword`;
    return this._http.post(url,requestModel).pipe();
  }  

  updatePassword(requestModel:UpdatePasswordRequestModel) {
    let url = `${this.baseUrl}UpdatePassword`;
    return this._http.put(url,requestModel).pipe();
  }
}
