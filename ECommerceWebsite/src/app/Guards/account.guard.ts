import { Injectable, inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Injectable({providedIn:"root"})
export class RegisterationService{

  constructor(private cookieService:CookieService, private router:Router) { }

  canActivate():boolean {
    if (this.cookieService.get('Token')) {
      this.router.navigate(['/']);
      return false;
    }else{
      return true;
    }
  }
}
export const accountGuard: CanActivateFn = (route, state) => {
  return inject(RegisterationService).canActivate();
};
