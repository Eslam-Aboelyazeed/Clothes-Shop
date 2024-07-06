import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmEmailService {

  baseUrl:string;

  constructor(private httpClient:HttpClient) { 
    this.baseUrl = "http://localhost:5259/api/confirmemail"
  }

  confirmEmail(uid:string, token:string){
    token = encodeURIComponent(token);
    console.log(token);
    return this.httpClient.get<boolean>(`${this.baseUrl}?userId=${uid}&token=${token}`);
  }
}
