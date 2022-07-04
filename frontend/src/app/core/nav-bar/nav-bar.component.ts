import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';
import { IAddress } from 'src/app/shared/models/address';
import { CartService } from 'src/app/cart/cart.service';
import { ICart } from 'src/app/shared/models/cart';
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  cart$: Observable<ICart> | undefined;
  currentUser$!: Observable<IUser>;
  constructor(private accountService: AccountService,private cartService: CartService) { }

  ngOnInit(): void {
    this.currentUser$=this.accountService.currentUser$;
    this.cart$=this.cartService.cart$;

  }

}
