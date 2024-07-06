import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUserLoginInsert } from '../Models/InsertModels/IUserLoginInsert';
import { IUserInsert } from '../Models/InsertModels/IUserInsert';
import { CookieService } from 'ngx-cookie-service';
import { IUser } from '../Models/DisplayModels/IUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl:string;

  headers:HttpHeaders;

  constructor(private httpClient:HttpClient, private cookieService:CookieService) { 
    this.baseUrl = "http://localhost:5259/api/";

    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.cookieService.get("Token")}`
    })
  }

  login(userInfo:IUserLoginInsert){
    return this.httpClient.post<{token:string}>(this.baseUrl+"login", userInfo);
  }

  logout(id:string){
    return this.httpClient.get<any>(this.baseUrl+"logout?UserId="+id, {headers: this.headers});
  }

  register(user:IUserInsert){
    return this.httpClient.post<any>(this.baseUrl+"register",user);
  }

  getUserInfo(userName:string){
    return this.httpClient.get<IUser>(this.baseUrl+`User?userName=${userName}`, {headers: this.headers})
  }

  forgetPassword(userEmail:string){
    return this.httpClient.get(this.baseUrl+`ForgetPassword?userEmail=${userEmail}`)
  }
}
