import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IManufacturer } from '../shared/models/manufacturer';
import { IPagination } from '../shared/models/pagination';
import { ICategory } from '../shared/models/category';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl=environment.apiUrl;
  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams)
  {
    let params=new HttpParams();
    if(shopParams.categoryID)
    {
      params=params.append('categoryID',shopParams.categoryID.toString());
    }
    if(shopParams.manufacturerID)
    {
      params=params.append('manufacturerID',shopParams.manufacturerID.toString());
    }
    if(shopParams.search)
    {
      params=params.append('search',shopParams.search)
    }
      params=params.append('sort',shopParams.sort);
      params=params.append('pageIndex',shopParams.pageNumber.toString())
      
    return this.http.get<IPagination>(this.baseUrl+'products',{observe:'response',params})
    .pipe(
      map(response => {
        return response.body
      })
    );
  }
  getProduct(productID: number){
    return this.http.get<IProduct>(this.baseUrl+'products/'+ productID);
  }
  getManufacturers()
  {
    return this.http.get<IManufacturer[]>(this.baseUrl+'manufacturers')
  }
  getCategories()
  {
    return this.http.get<ICategory[]>(this.baseUrl+"categories")
  }

  
}
