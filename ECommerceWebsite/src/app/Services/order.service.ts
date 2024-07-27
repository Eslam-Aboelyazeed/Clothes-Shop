import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IOrderInsert } from '../Models/InsertModels/IOrderInsert';
import { IOrder } from '../Models/DisplayModels/IOrder';
import { IOrderProductsInsert } from '../Models/InsertModels/IOrderProductsInsert';
import { IOrderProducts } from '../Models/DisplayModels/IOrderProducts';
import { IOrderProductsUpdate } from '../Models/UpdateModels/IOrderProductsUpdate';
import { IOrderUpdate } from '../Models/UpdateModels/IOrderUpdate';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  // headers:HttpHeaders;
  // constructor(private httpClient:HttpClient, private cookieService:CookieService) {
  //   this.headers = new HttpHeaders({
  //     'Authorization': `Bearer ${this.cookieService.get("Token")}`,
  //     'Access-Control-Allow-Origin': '*'
  //   })
  //  }

  // GetAll(){
  //   return this.httpClient.get<IOrder[]>('https://localhost:7012/api/Order', {headers: this.headers})
  // }

  // GetById(id:number){
  //   return this.httpClient.get<IOrder | undefined>(`https://localhost:7012/api/Order/${id}`, {headers: this.headers});
  // }

  // addOrder(element:IOrderInsert){
  //   return this.httpClient.post<IOrder>("https://localhost:7012/api/Order",element, {headers: this.headers})
  // }

  // addOrderProduct(element:IOrderProductsInsert){
  //   return this.httpClient.post<IOrderProducts>('https://localhost:7012/api/OrderProducts', element, {headers: this.headers});
  // }

  // updateOrderProduct(oid:number, element:IOrderProductsUpdate){
  //   return this.httpClient.put(`https://localhost:7012/api/OrderProducts/${oid}`, element, {headers: this.headers});
  // }

  // updateOrderProducts(oid:number, element:IOrder){
  //   return this.httpClient.put(`https://localhost:7012/api/ordrProducts/${oid}`, element, {headers: this.headers});
  // }

  // Edit(id:number,element:IOrderUpdate){
  //   return this.httpClient.put(`https://localhost:7012/api/Order/${id}`, element, {headers: this.headers});
  // }

  // Delete(oid:number, pid:number){
  //   return this.httpClient.delete(`https://localhost:7012/api/OrderProducts/${oid}?pid=${pid}`, {headers: this.headers});
  // }

  // DeleteOrder(oid:number){
  //   return this.httpClient.delete(`https://localhost:7012/api/Order/${oid}`, {headers: this.headers});
  // }

  // getUserOrders(userId:string){
  //   return this.httpClient.get<IOrder[]>(`https://localhost:7012/api/Ordr?userId=${userId}`, {headers: this.headers})
  // }

  // getOrderCount(userId:string){
  //   return this.httpClient.get<number>(`https://localhost:7012/api/ordercount?userId=${userId}`, {headers: this.headers})
  // }

  constructor(private httpClient:HttpClient) {
   }

  GetAll(){
    return this.httpClient.get<IOrder[]>('https://localhost:7012/api/Order')
  }

  GetById(id:number){
    return this.httpClient.get<IOrder | undefined>(`https://localhost:7012/api/Order/${id}`);
  }

  addOrder(element:IOrderInsert){
    return this.httpClient.post<IOrder>("https://localhost:7012/api/Order",element)
  }

  addOrderProduct(element:IOrderProductsInsert){
    return this.httpClient.post<IOrderProducts>('https://localhost:7012/api/OrderProducts', element);
  }

  updateOrderProduct(oid:number, element:IOrderProductsUpdate){
    return this.httpClient.put(`https://localhost:7012/api/OrderProducts/${oid}`, element);
  }

  updateOrderProducts(oid:number, element:IOrder){
    return this.httpClient.put(`https://localhost:7012/api/ordrProducts/${oid}`, element);
  }

  Edit(id:number,element:IOrderUpdate){
    return this.httpClient.put(`https://localhost:7012/api/Order/${id}`, element);
  }

  Delete(oid:number, pid:number){
    return this.httpClient.delete(`https://localhost:7012/api/OrderProducts/${oid}?pid=${pid}`);
  }

  DeleteOrder(oid:number){
    return this.httpClient.delete(`https://localhost:7012/api/Order/${oid}`);
  }

  getUserOrders(userId:string){
    return this.httpClient.get<IOrder[]>(`https://localhost:7012/api/Ordr?userId=${userId}`)
  }

  getOrderCount(userId:string){
    return this.httpClient.get<number>(`https://localhost:7012/api/ordercount?userId=${userId}`)
  }
}
