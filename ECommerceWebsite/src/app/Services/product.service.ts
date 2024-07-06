import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProductInsert } from '../Models/InsertModels/IProductInsert';
import { IProduct } from '../Models/DisplayModels/IProduct';
import { IProductPage } from '../Models/DisplayModels/IProductPage';
import { IFilterOptions } from '../Models/DisplayModels/IFilterOptions';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseUrl:string;
  headers:HttpHeaders;
  constructor(private httpClient:HttpClient, private cookieService:CookieService) {
    this.baseUrl = 'http://localhost:5259/api/product';
    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.cookieService.get("Token")}`
    })
  }

  getProductPages(page:number = 1, pageSize:number = 5, filterOptions:IFilterOptions|null = null){
    return this.httpClient.post<IProductPage>(`${this.baseUrl}s?page=${page}&pageSize=${pageSize}`, filterOptions, {headers: this.headers});
  }

  getProductsBySearch(name:string){
    return this.httpClient.get<IProduct[]>(`${this.baseUrl}?q=${name}`, {headers: this.headers})
  }

  addproduct(element:FormData){
    return this.httpClient.post<IProduct>(this.baseUrl, element, {headers: this.headers});
  }

  updateproduct(id:number, element:FormData){
    return this.httpClient.put<IProduct>(`${this.baseUrl}/${id}`, element, {headers: this.headers});
  }

  deleteproduct(id:number){
    return this.httpClient.delete(`${this.baseUrl}/${id}`, {headers: this.headers});
  }
}
