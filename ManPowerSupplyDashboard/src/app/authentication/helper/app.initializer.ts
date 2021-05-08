import { AuthenticationService } from "../services/authentication.service";

export function appInitializer(authenticationService: AuthenticationService) {
    return () => new Promise(resolve => {
        // attempt to refresh token on app start up to auto authenticate
        authenticationService.refreshToken()
            .subscribe(x=>{localStorage.setItem('token',x.token)})
            .add(resolve);
    });
}