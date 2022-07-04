import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CartService } from 'src/app/cart/cart.service';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product!: IProduct;
  test!: Number;
  quantity = 1;
  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private cartService: CartService) { }

  ngOnInit(): void {
    this.loadProduct();
  }
  addItemToCart(){
    this.cartService.addItemToCart(this.product,this.quantity)
  }
  incrementQuantity(){
    this.quantity++;
  }
  decrementQuantity(){
    if(this.quantity>1)
    this.quantity--;
  }
  loadProduct()
  {
       this.test=this.activatedRoute.snapshot.params['id'];
      this.shopService.getProduct(+this.test).subscribe(product=>{ 
        this.product=product; 
      }, error=>{
        console.log(error);
      }
      )
    
   
  }

}
