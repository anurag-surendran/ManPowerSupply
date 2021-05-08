import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticationService } from "../services/authentication.service";

import { environment } from 'src/environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add auth header with jwt if user is logged in and request is to the api url
        const user = this.authenticationService.userValue;
        const isLoggedIn = user && user.token;
        let manPowerBaseUri = `${environment.apiUrl}ManPower/`;
        let userManagerBaseUri = `${environment.apiUrl}UserManager/`;
        let dashboardBaseUri = `${environment.apiUrl}Dashboard/`;
        const isApiUrl = request.url.startsWith(manPowerBaseUri)
            || request.url.startsWith(userManagerBaseUri)
            || request.url.startsWith(dashboardBaseUri);

        if (isLoggedIn && isApiUrl) {
            let organizationId = localStorage.getItem('organizationId');
            request = request.clone({
                setHeaders: { Authorization: `Bearer ${user.token}`, OrganizationId: organizationId },
                withCredentials: true
            });
        }

        return next.handle(request);
    }
}