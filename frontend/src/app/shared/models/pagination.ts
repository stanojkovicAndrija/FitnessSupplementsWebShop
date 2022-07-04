import { IProduct } from "./product";

export interface IPagination {
    products: IProduct[];
    pageSize: number;
    pageIndex: number;
    count: number;
}