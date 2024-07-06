import { ProductType } from "../ProductTypes";
import { IProductReviews } from "./IProductReviews";
import { IProduct } from "./IProduct";

export interface IProductPage{
    products: IProduct[],
    totalCount: number,
    totalPages: number
}