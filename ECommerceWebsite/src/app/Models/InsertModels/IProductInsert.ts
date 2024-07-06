import { ProductType } from "../ProductTypes";

export interface IProductInsert{
    name:string,
    type:ProductType,
    description:string,
    price:number,
    quantity:number,
    rating:number,
    imageUrl:FormData
}