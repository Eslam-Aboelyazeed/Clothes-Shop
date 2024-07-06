import { ProductType } from "../ProductTypes";

export interface IOrderProducts{
    oId:number,
    pId:number,
    productName:string,
    productType:ProductType,
    productPrice:number,
    productImage:string,
    productQuantity:number,
    totalPrice:number,
    quantity:number
}