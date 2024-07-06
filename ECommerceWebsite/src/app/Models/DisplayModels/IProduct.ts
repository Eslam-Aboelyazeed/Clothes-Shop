import { ProductType } from "../ProductTypes";
import { IProductReviews } from "./IProductReviews";

export interface IProduct{
    id:number,
    name:string,
    type:ProductType,
    description:string,
    price:number,
    quantity:number,
    rating:number,
    imageUrl:string,
    isSpecialOffer:boolean,
    newPrice:number,
    expireDate:Date,
    productReviews: IProductReviews[]
}