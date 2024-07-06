import { Component, Input, NgModule, OnDestroy, OnInit } from '@angular/core';
import { ProductItemComponent } from '../product-item/product-item.component';
import { IProduct } from '../../Models/DisplayModels/IProduct';
import { GenericService } from '../../Services/generic.service';
import { ProductService } from '../../Services/product.service';
import { IProductPage } from '../../Models/DisplayModels/IProductPage';
import { ProductType } from '../../Models/ProductTypes';
import { FormsModule } from '@angular/forms';
import { IFilterOptions } from '../../Models/DisplayModels/IFilterOptions';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [ProductItemComponent, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit, OnDestroy{

  products:IProductPage | undefined;

  productList: IProduct[];

  proSub:any;

  pagesList: number[];

  currentPage:number;

  proTypes:any;

  options:string[];

  maxPrice:number;

  minPrice:number;

  filterOptions:IFilterOptions;

  totalCount:number;

  productName:string;

  constructor(private productService:ProductService){
    this.productList = [];
    this.pagesList = [];
    this.currentPage = 1;
    this.totalCount = 0;

    this.minPrice = 0;
    this.maxPrice = 10000;

    this.productName = '';

    this.proTypes = ProductType;

    this.options = Object.keys(this.proTypes).map(key => this.proTypes[key]);

    this.options = this.options.filter(el => typeof(el) != "number" )

    this.filterOptions = {
      tShirts:true,
      shoes:true,
      jackets:true,
      other:true,
      maxPrice:this.maxPrice,
      minPrice:this.minPrice,
      fiveStars:true,
      fourStars:true,
      threeStars:true,
      twoStars:true,
      oneStar:true,
      name:''
    }
  }

  ngOnInit(): void {
    this.proSub = this.productService.getProductPages().subscribe({
      next: data => {
        this.products = data;
        this.productList = this.products.products;
        this.pagesList = [];
        for (let i = 0; i < this.products.totalPages; i++) {
          this.pagesList.push(i+1);
        }
        this.totalCount = this.products.totalCount;
      },
      error: error => console.log(error)
    })
  }

  ngOnDestroy(): void {
    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }
  }

  switchPages(i:number){
    if (i == 0) {
      this.currentPage = this.currentPage + 1;
    }else if (i == -1) {
      this.currentPage = this.currentPage - 1;
    }else{
      this.currentPage = i;
    }


    this.proSub = this.productService.getProductPages(this.currentPage).subscribe({
      next: data => {
        this.products = data;
        this.productList = this.products.products;
        this.pagesList = [];
        for (let i = 0; i < this.products.totalPages; i++) {
          this.pagesList.push(i+1);
        }
      },
      error: error => console.log(error)
    })
  }

  toggleFilter(category:string, event:Event){

    if ((event.target as HTMLInputElement).checked) {
      this.filterOptions = {
        ...this.filterOptions,
        [category.toLowerCase()]:true
      }
    } else {
      this.filterOptions = {
        ...this.filterOptions,
        [category.toLowerCase()]:false
      }
    }
  }

  changePriceFilter(filter: string){

    if (filter === 'higher') {
      this.filterOptions = {
        ...this.filterOptions,
        minPrice:this.minPrice
      }
    } else if(filter === 'lower') {
      this.filterOptions = {
        ...this.filterOptions,
        maxPrice:this.maxPrice
      }
    }
  }

  toggleRatingFilter(rating:string, event:Event){
    if ((event.target as HTMLInputElement).checked) {
      this.filterOptions = {
        ...this.filterOptions,
        [rating]:true
      }
    } else {
      this.filterOptions = {
        ...this.filterOptions,
        [rating]:false
      }
    }
  }

  applyFilter() {
    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }

    this.proSub = this.productService.getProductPages(1, 5, this.filterOptions).subscribe({
      next: data => {
        this.products = data;
        this.productList = this.products.products;
        this.pagesList = [];
        for (let i = 0; i < this.products.totalPages; i++) {
          this.pagesList.push(i+1);
        }
        this.totalCount = this.products.totalCount;
      },
      error: error => console.log(error)
    })
  }

  searchByName(){

    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }

    this.filterOptions = {
      ...this.filterOptions,
      name:this.productName
    }

    this.proSub = this.productService.getProductPages(1, 5, this.filterOptions).subscribe({
      next: data => {
        this.products = data;
        this.productList = this.products.products;
        this.pagesList = [];
        for (let i = 0; i < this.products.totalPages; i++) {
          this.pagesList.push(i+1);
        }
        this.totalCount = this.products.totalCount;
      },
      error: error => console.log(error)
    })
  }
}
