import { ICategory } from "./category";
import { IManufacturer } from "./manufacturer";

    export interface IProduct {
        productID: number;
        name: string;
        pictureUrl: string;
        description: string;
        price: number;
        quantity: number;
        categoryID: number;
        category: ICategory;
        manufacturerID: number;
        manufacturer: IManufacturer;
    }



