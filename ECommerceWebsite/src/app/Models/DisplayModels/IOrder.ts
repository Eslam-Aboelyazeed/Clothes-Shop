import { IOrderCoupon } from "./IOrderCoupon";
import { IOrderProducts } from "./IOrderProducts";

export interface IOrder{
    id:number,
    status:string,
    totalPrice:number,
    orderDate:Date,
    userId:string,
    userName:string,
    userEmail:string,
    orderProducts:IOrderProducts[],
    orderCoupons:IOrderCoupon[]
}