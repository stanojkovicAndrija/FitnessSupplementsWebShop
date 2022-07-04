import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ICategory } from '../shared/models/category';
import { IManufacturer } from '../shared/models/manufacturer';
import { IProduct } from '../shared/models/product';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search', { static: true })
  searchTerm!: ElementRef;
  products: IProduct[]=[];
  manufacturers: IManufacturer[]=[];
  categories: ICategory[]=[];
  shopParams = new ShopParams();
  totalCount!: number;
  sortOptions=[
    {name:'Ime, od Z do A', value: 'nameDesc'},
    {name:'Cena, od najnize do najvise', value: 'priceAsc'},
    {name:'Cena, od najvise do najnize', value: 'priceDesc'}
  ]
  constructor(private shopService: ShopService) { }

  ngOnInit() {
    this.getProducts();
    this.getCategories();
    this.getManufacturers();
  }
  getProducts()
  {
    this.shopService.getProducts(this.shopParams).subscribe(response =>
      {

          this.products=response!.products
          this.shopParams.pageNumber=response!.pageIndex;
          this.totalCount=response!.count;
          this.shopParams.pageSize=response!.pageSize;

      }, error => {
        console.log(error);
      });
  }
  getManufacturers()
  {
    this.shopService.getManufacturers().subscribe(response =>
      {
        this.manufacturers=[{manufacturerID: 0, name: 'All'}, ...response];
      }, error => {
        console.log(error);
      });
  }
  getCategories()
  {
    this.shopService.getCategories().subscribe(response =>
      {
        this.categories=[{categoryID: 0, name: 'All'}, ...response];
      }, error => {
        console.log(error);
      });
  }

  onCategorySelected(categoryID: number)
  {
    this.shopParams.categoryID=categoryID;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }

  onManufacturerSelected(manufacturerID: number)
  {
    this.shopParams.manufacturerID=manufacturerID;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }

  onSortSelected(sort: string)
  {
    this.shopParams.sort=sort;
    this.getProducts();
  }
  
  onPageChanged(event:any)
  {
    if(this.shopParams.pageNumber!== event)
    {
      this.shopParams.pageNumber=event;
      this.getProducts();
    }

  }
  
  onSearch()
  {
    this.shopParams.search=this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber=1;
    this.getProducts();
  }
  onReset()
  {
    this.searchTerm.nativeElement.value="";
    this.shopParams=new ShopParams();
    this.getProducts();
  }
}
