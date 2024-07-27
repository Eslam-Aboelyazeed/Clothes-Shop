import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class GenericService<T> {
  // baseUrl:string;
  // headers:HttpHeaders;
  // constructor(private httpClient:HttpClient, private cookieService:CookieService) { 
  //   this.baseUrl = 'https://localhost:7012/api/';

  //   this.headers = new HttpHeaders({
  //     'Authorization': `Bearer ${this.cookieService.get("Token")}`,
  //     'Access-Control-Allow-Origin': '*'
  //   });
  // }

  // GetAll(){
  //   console.log(this.headers);
    
  //   return this.httpClient.get<T[]>('https://localhost:7012/api/'+this.baseUrl, {headers: this.headers})
  // }

  // GetById(id:number){
  //   return this.httpClient.get<T | undefined>(`https://localhost:7012/api/${this.baseUrl}/${id}`, {headers: this.headers});
  // }

  // Add(element:T){
  //   return this.httpClient.post<T>('https://localhost:7012/api/'+this.baseUrl, element, {headers: this.headers});
  // }

  // Edit(id:number,element:T){
  //   return this.httpClient.put(`https://localhost:7012/api/${this.baseUrl}/${id}`, element, {headers: this.headers});
  // }

  // Delete(id:number){
  //   return this.httpClient.delete(`https://localhost:7012/api/${this.baseUrl}/${id}`, {headers: this.headers});
  // }

  baseUrl:string;
  constructor(private httpClient:HttpClient) { 
    this.baseUrl = 'https://localhost:7012/api/';
  }

  GetAll(){
    return this.httpClient.get<T[]>('https://localhost:7012/api/'+this.baseUrl)
  }

  GetById(id:number){
    return this.httpClient.get<T | undefined>(`https://localhost:7012/api/${this.baseUrl}/${id}`);
  }

  Add(element:T){
    return this.httpClient.post<T>('https://localhost:7012/api/'+this.baseUrl, element);
  }

  Edit(id:number,element:T){
    return this.httpClient.put(`https://localhost:7012/api/${this.baseUrl}/${id}`, element);
  }

  Delete(id:number){
    return this.httpClient.delete(`https://localhost:7012/api/${this.baseUrl}/${id}`);
  }
}
