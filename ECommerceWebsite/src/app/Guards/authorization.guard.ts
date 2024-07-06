import { Injectable, inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Injectable({providedIn: 'root'})
  export class AuthorizationService {

  constructor(
  private cookieService: CookieService,
  public router: Router,
  ) { }

  canActivate(): boolean {
    if (this.cookieService.get('Token')) {
      if ((localStorage.getItem("Role") && localStorage.getItem("Role") == "Admin") || (this.cookieService.get("Role") && this.cookieService.get("Role") == "Admin")) {
        return true
      }else if((localStorage.getItem("Role") && localStorage.getItem("Role") == "User") || (this.cookieService.get("Role") && this.cookieService.get("Role") == "User")){
        this.router.navigate(['/']);
        return false;
      }
      return false;
    } else {
      this.router.navigate(['/login']);
      return false
    }
  }

}

export const authorizationGuard: CanActivateFn = (route, state) => {
  return inject(AuthorizationService).canActivate();
};
