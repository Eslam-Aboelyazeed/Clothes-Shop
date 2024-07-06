import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class GenericService<T> {
  baseUrl:string;
  headers:HttpHeaders;
  constructor(private httpClient:HttpClient, private cookieService:CookieService) { 
    this.baseUrl = 'http://localhost:5259/api/';

    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.cookieService.get("Token")}`,
      'Access-Control-Allow-Origin': '*'
    });
  }

  GetAll(){
    console.log(this.headers);
    
    return this.httpClient.get<T[]>('http://localhost:5259/api/'+this.baseUrl, {headers: this.headers})
  }

  GetById(id:number){
    return this.httpClient.get<T | undefined>(`http://localhost:5259/api/${this.baseUrl}/${id}`, {headers: this.headers});
  }

  Add(element:T){
    return this.httpClient.post<T>('http://localhost:5259/api/'+this.baseUrl, element, {headers: this.headers});
  }

  Edit(id:number,element:T){
    return this.httpClient.put(`http://localhost:5259/api/${this.baseUrl}/${id}`, element, {headers: this.headers});
  }

  Delete(id:number){
    return this.httpClient.delete(`http://localhost:5259/api/${this.baseUrl}/${id}`, {headers: this.headers});
  }
}
