import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrganizationModel, User } from '../models/user-model';

import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private userSubject: BehaviorSubject<User>;
  public user: Observable<User>;
  private baseUrl: string = `${environment.apiUrl}User/`;

  constructor(private router: Router,
    private http: HttpClient) {
    this.userSubject = new BehaviorSubject<User>(null);
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

  login(username: string, password: string) {

    let login = {
      username: username,
      password: password
    }

    return this.http.post<any>(`${this.baseUrl}Authenticate`, login, { withCredentials: true })
      .pipe(map((user: User) => {
        this.userSubject.next(user);
        localStorage.setItem("organizationId", user.organizations[0].id.toString());
        localStorage.setItem('token', user.token);
        this.startRefreshTokenTimer();
        return user;
      }));
  }

  logout() {
    this.http.post<any>(`${this.baseUrl}RevokeToken`, {}, { withCredentials: true }).subscribe();
    this.stopRefreshTokenTimer();
    this.userSubject.next(null);
    localStorage.removeItem('token');
    localStorage.removeItem('organizationId');
    this.router.navigate(['/login']);
  }

  setOrganization(organization: OrganizationModel, token: string) {
    let headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    localStorage.setItem("organizationId", organization.organizationId.toString());
    return this.http.post<any>(`${this.baseUrl}SetOrganization`, { organizationId: organization.organizationId }, { headers, withCredentials: true })
      .pipe();
  }

  refreshToken() {
    let token = this.userValue == null ? "" : this.userValue.token;
    if (token == "")
      token = localStorage.getItem('token');
    let headers = new HttpHeaders({
      'Authorization': `${token ==null ? "" : "Bearer "+token}`
    });
    return this.http.post<any>(`${this.baseUrl}RefreshToken`, null, { headers, withCredentials: true })
      .pipe(map((user: User) => {
        this.userSubject.next(user);
        let organizationId = localStorage.getItem('token');
        if (organizationId == null || organizationId == undefined)
          localStorage.setItem("organizationId", user.organizations[0].id.toString());
        this.startRefreshTokenTimer();
        return user;
      }));
  }

  private refreshTokenTimeout;

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    const jwtToken = JSON.parse(atob(this.userValue.token.split('.')[1]));

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(x => { localStorage.setItem('token', x.token) }), timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
