import { ProductType } from "../ProductTypes";

export interface IProductUpdate{
    id:number,
    name:string,
    type:ProductType,
    description:string,
    price:number,
    quantity:number,
    rating:number,
    imageUrl:FormData
}