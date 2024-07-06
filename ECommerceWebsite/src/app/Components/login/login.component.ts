import { Router, RouterLink } from '@angular/router';
import { Component, NgModule, OnDestroy, importProvidersFrom } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericService } from '../../Services/generic.service';
import { IUserLoginInsert } from '../../Models/InsertModels/IUserLoginInsert';
import Swal from 'sweetalert2';
import { AccountService } from '../../Services/account.service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from '../../app.component';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, JwtModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnDestroy {
  form:FormGroup;
  aSub:any;
  fSub:any;

  constructor(private accountService:AccountService, private cookieService:CookieService, private router:Router){
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      isPersistent: new FormControl(false)
    });

    JwtModule.forRoot({
      config: {
        tokenGetter: undefined,
        allowedDomains: ["example.com"],
        disallowedRoutes: ["http://example.com/examplebadroute/"],
      },
    })
  }

  ngOnDestroy(): void {
    if (this.aSub != undefined) {
      this.aSub.unsubscribe();
    }

    if (this.fSub != undefined) {
      this.fSub.unsubscribe();
    }
  }

  get getEmail(){
    return this.form.controls['email'];
  }

  get getPassword(){
    return this.form.controls['password'];
  }

  get getIsPersistent(){
    return this.form.controls['isPersistent'];
  }

  login(){
    if (this.form.status == 'INVALID') {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Please Input All the Required Fields with Valid Values",
      })
      return;
    }

    this.aSub = this.accountService.login(this.form.value).subscribe({
      next: data => {
        console.log(data.token);
        let jwtHelper:JwtHelperService = new JwtHelperService();
        let token = jwtHelper.decodeToken(data.token);
        console.log(token);
        localStorage.setItem("RememberMe", this.getIsPersistent.value);
        if (this.getIsPersistent.value) {
          this.cookieService.set("Token",data.token, 1);
          localStorage.setItem("UserId", token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']);
          localStorage.setItem("UserName", token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']);
          localStorage.setItem("Role", token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
          localStorage.setItem("Email", this.getEmail.value);
          localStorage.setItem("Password", this.getPassword.value);        
        }else{
          this.cookieService.set("Token",data.token, 1);
          this.cookieService.set("UserId",token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'], 1);
          this.cookieService.set("UserName",token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'], 1);
          this.cookieService.set("Role",token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'], 1);
          this.cookieService.set("ExpireDate",(new Date().getDate()+1).toString(), 1);
        }


        if (token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === "User") {
          this.router.navigate(["/"])
        }else if (token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === "Admin"){
          this.router.navigate(["/admindashboard"])
        }
      },
      error: error => {
        console.log(error);
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Invaild Email or Password!"
        });
      }
    })
  }

  forgetPassword(){

    if (this.getEmail.status == 'INVALID') {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Please Input your email",
      })
      return;
    }

    this.fSub = this.accountService.forgetPassword(this.getEmail.value).subscribe({
      next: data =>{
        console.log(data);

        Swal.fire({
          icon: "info",
          title: "New Password",
          text: "Check your Email Account"
        });
      },
      error: error => {
        console.log(error);
      }
    })
  }
}
