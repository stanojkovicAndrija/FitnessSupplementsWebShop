import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Cart, ICart, ICartItem, ICartTotals } from '../shared/models/cart';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl= environment.apiUrl;
  private emptyTotals!: ICartTotals;
  private emptyCart!: ICart;
  private cartTotalSource=new BehaviorSubject<ICartTotals>(this.emptyTotals);
  private cartSource= new BehaviorSubject<ICart>(this.emptyCart);

  cart$= this.cartSource.asObservable();
  cartTotal$=this.cartTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getCart(id: string) {
    return this.http.get(this.baseUrl + 'cart?shoppingCartID=' + id)
      .pipe(
        map((cart: any) => {
          this.cartSource.next(cart);
          this.calculateTotals();
        })
      )
  }

  setCart(cart: ICart)
  {
    return this.http.post(this.baseUrl+'cart',cart).subscribe((response:any)=>
    {
      this.cartSource.next(response);
      this.calculateTotals();
    },error=>{
      console.log(error);
    });
  }
  incrementItemQuantity(item: ICartItem)
  {
    const cart = this.getCurrentCartValue();
    const foundItemIndex= cart.items.findIndex(x => x.productID === item.productID)
    cart.items[foundItemIndex].quantity++;
    this.setCart(cart);
  }
  decrementItemQuantity(item: ICartItem)
  {
    const cart = this.getCurrentCartValue();
    const foundItemIndex= cart.items.findIndex(x => x.productID === item.productID)
    if(cart.items[foundItemIndex].quantity>1)
    {
      cart.items[foundItemIndex].quantity--;
      this.setCart(cart);
    }else{
      this.removeItemFromCart(item);
    }
  }
   removeItemFromCart(item: ICartItem) {
    const cart=this.getCurrentCartValue();
    if(cart.items.some(x=>x.productID==item.productID)){
      cart.items=cart.items.filter(i=> i.productID !== item.productID)
      if(cart.items.length>0){
        this.setCart(cart);
      }else
      {
        this.deleteCart(cart);
      }
    }
  }
  deleteCart(cart: ICart) {
    
    return this.http.delete(this.baseUrl+'cart?shoppingCartID'+cart.shoppingCartID).subscribe(()=>{
      this.cartSource.next(this.emptyCart);
      this.cartTotalSource.next(this.emptyTotals);
      localStorage.removeItem("cart_id");
    },error => {
      console.log(error)
    })
  }
  getCurrentCartValue()
  {
    return this.cartSource.value;
  }

  addItemToCart(item: IProduct,quantity=1)
  {
    const itemToAdd: ICartItem=item;
    const cart = this.getCurrentCartValue() ?? this.createCart();
    cart.items = this.addOrUpdateItem(cart.items,itemToAdd,quantity);
    this.setCart(cart);
  }
  private addOrUpdateItem(items: ICartItem[], itemToAdd: ICartItem, quantity: number): ICartItem[] {
  
    const index = items.findIndex(i => i.productID=== itemToAdd.productID)
    if(index === -1)
    {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    else
    {
      items[index].quantity+=quantity;
    }
    return items;
  }
  private createCart(): ICart {
   const cart= new Cart();
   localStorage.setItem('cart_id ',cart.shoppingCartID);
   return cart;
  }
  private calculateTotals()
  {
    const cart=this.getCurrentCartValue();
    const delivery=0;
    const subtotal=cart.items.reduce((a,b)=>(b.price * b.quantity) + a, 0);
    const total = subtotal+delivery;
    this.cartTotalSource.next({delivery,total,subtotal})
  }
  //dodati mapping ako ne radi.
}
