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
  constructor(private httpClient:HttpClient) {
    this.baseUrl = 'https://localhost:7012/api/product';
  }

  getProductPages(page:number = 1, pageSize:number = 5, filterOptions:IFilterOptions|null = null){
    return this.httpClient.post<IProductPage>(`${this.baseUrl}s?page=${page}&pageSize=${pageSize}`, filterOptions);
  }

  getProductsBySearch(name:string){
    return this.httpClient.get<IProduct[]>(`${this.baseUrl}?q=${name}`)
  }

  addproduct(element:FormData){
    return this.httpClient.post<IProduct>(this.baseUrl, element);
  }

  updateproduct(id:number, element:FormData){
    return this.httpClient.put<IProduct>(`${this.baseUrl}/${id}`, element);
  }

  deleteproduct(id:number){
    return this.httpClient.delete(`${this.baseUrl}/${id}`);
  }
}
