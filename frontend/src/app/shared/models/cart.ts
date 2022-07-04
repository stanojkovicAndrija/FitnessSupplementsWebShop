import { v4 as uuidv4} from 'uuid';

    export interface ICartItem {
        productID: number;
        name: string;
        pictureUrl: string;
        quantity: number;
        price: number;
    }

    export interface ICart {
        shoppingCartID: string;
        items: ICartItem[];
    }

    export class Cart implements ICart{
        shoppingCartID = uuidv4();
        items: ICartItem[]=[];
    }

    export interface ICartTotals{
        delivery:number;
        subtotal:number;
        total:number;
    }


