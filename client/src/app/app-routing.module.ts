import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { ShopComponent } from './shop/shop.component';

const routes: Routes = [  

  {path: '', component: ShopComponent},
  {path: 'products/:id', component: ProductDetailsComponent},
  {path: 'account', loadChildren:() =>import('./account/account.module').then (mod=>mod.AccountModule)},
  {path: 'cart', loadChildren:() =>import('./cart/cart.module').then (mod=>mod.CartModule)},
  {path: '**', redirectTo:'', pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
