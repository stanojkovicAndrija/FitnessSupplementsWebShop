
import { Component, OnInit } from '@angular/core';
import { CartService } from './cart/cart.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'FitnessWebShop';

  constructor(private cartService: CartService) {}
  
  ngOnInit(): void {
    const cartID=localStorage.getItem('cart_id')
    if(cartID)
  {
    this.cartService.getCart(cartID).subscribe(()=>{
      console.log('initialised cart');
    },error =>{
      console.log(error);
    });
     
  }
  }

}
