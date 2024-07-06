import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { AddProductComponent } from '../add-product/add-product.component';
import { GenericService } from '../../Services/generic.service';
import { IProduct } from '../../Models/DisplayModels/IProduct';
import Swal from 'sweetalert2';
import { ProductType } from '../../Models/ProductTypes';
import { CommonModule } from '@angular/common';
import { UpdateProductComponent } from '../update-product/update-product.component';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from '../../Services/account.service';
import { Router } from '@angular/router';
import { ProductService } from '../../Services/product.service';
import { IFilterOptions } from '../../Models/DisplayModels/IFilterOptions';
import { IProductPage } from '../../Models/DisplayModels/IProductPage';
import { FormsModule } from '@angular/forms';

import {MatChipsModule} from '@angular/material/chips';

@Component({
  selector: 'app-admin-dash-board',
  standalone: true,
  imports: [AddProductComponent, CommonModule, UpdateProductComponent, FormsModule, MatChipsModule],
  templateUrl: './admin-dash-board.component.html',
  styleUrl: './admin-dash-board.component.css'
})
export class AdminDashBoardComponent implements OnInit, OnDestroy {
  
  toggleLink:boolean;

  homeHoverLink:boolean;

  activeLink:string;

  proSub:any;

  dSub:any;

  lSub:any;

  products:IProductPage | undefined;

  productList:IProduct[];

  filterOptions:IFilterOptions;

  proId:number;

  pagesList: number[];

  currentPage:number;

  totalCount:number;

  productName:string;

  proTypes:any;

  categories:string[];

  filterText:string;

  constructor(private productService:ProductService, 
    private cookieService:CookieService, 
    private accountService:AccountService, 
    private router:Router){

    this.homeHoverLink = false;
    this.toggleLink = false;
    this.activeLink = 'home';
    this.productList = [];
    this.pagesList = [];
    this.currentPage = 1;
    this.totalCount = 0;

    this.proId = 0;

    this.productName = '';

    this.filterOptions = {
      tShirts:true,
      shoes:true,
      jackets:true,
      other:true,
      maxPrice:10000,
      minPrice:0,
      fiveStars:true,
      fourStars:true,
      threeStars:true,
      twoStars:true,
      oneStar:true,
      name:''
    }

    this.proTypes = ProductType;

    this.categories = Object.keys(this.proTypes).map(key => this.proTypes[key]);

    this.categories = this.categories.filter(el => typeof(el) != "number" );

    this.filterText = 'All Products';
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
    if (this.dSub != undefined) {
      this.dSub.unsubscribe();
    }
    if (this.lSub != undefined) {
      this.lSub.unsubscribe();
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

  linkHovered(){

  }

  HomeLinkHovered(){
    this.homeHoverLink == false ?this.homeHoverLink = true:this.homeHoverLink = false;
    console.log("test");
  }

  toggle(){
    this.toggleLink == false ?this.toggleLink = true:this.toggleLink = false;
  }

  changeActive(link:string){
    this.activeLink = link;
  }

  deleteProduct(id:number){
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.dSub = this.productService.deleteproduct(id).subscribe({
          next: data => {
            this.productList = this.productList.filter(el => el.id != id);
            Swal.fire({
              title: "Deleted!",
              text: "Product has been deleted.",
              icon: "success"
            });
          },
          error: error => {
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "Something went wrong! Please try again later"
            });
          }
        })
      }
    });
  }

  getKeyByValue(value: any) {
    return ProductType[value];
  }

  editProduct(id:number){
    this.activeLink = "update";
    this.proId = id;
  }

  addProduct(product: IProduct){
    this.activeLink = 'home';
    this.productList.push(product); 
  }

  updateProduct(product: IProduct){
    this.activeLink = 'home';
    let p = this.productList.find(p => p.id === product.id);
    // p = product;
    if (p) {
      Object.assign(p, product);
    }
  }

  Logout(){

    Swal.fire({
      title: "Are you sure you want to logout?",
      icon: "question",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Logout"
    }).then((result) => {
      if (result.isConfirmed) {
        let uid;
        if (localStorage.getItem("RememberMe") === "true") {
          uid = localStorage.getItem("UserId") || "";
        }else{
          uid = this.cookieService.get("UserId");
        }
    
        this.lSub = this.accountService.logout(uid).subscribe({
          next: data => {
            this.cookieService.deleteAll();
            localStorage.clear();
            this.router.navigate(['/']);
          },
          error: error => console.log(error)
        });
        
      }
    });
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

  filter(category:string){
    switch (category) {
      case "TShirts":
        this.filterOptions = {
          ...this.filterOptions,
          tShirts:true,
          shoes:false,
          jackets:false,
          other:false
        };
        this.filterText = category;
        break;

      case "Jackets":
        this.filterOptions = {
          ...this.filterOptions,
          shoes:false,
          tShirts:false,
          jackets:true,
          other:false
        };
        this.filterText = category;
        break;

      case "Shoes":
        this.filterOptions = {
          ...this.filterOptions,
          tShirts:false,
          jackets:false,
          shoes:true,
          other:false
        };
        this.filterText = category;
        break;

      case "Other":
        this.filterOptions = {
          ...this.filterOptions,
          shoes:false,
          jackets:false,
          tShirts:false,
          other:true
        };
        this.filterText = category;
        break;
    
      default:
        this.filterOptions = {
          ...this.filterOptions,
          tShirts:true,
          shoes:true,
          jackets:true,
          other:true
        };
        this.filterText = 'All Products';
        break;
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
