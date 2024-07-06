import { IProduct } from './../../Models/DisplayModels/IProduct';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { GenericService } from '../../Services/generic.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductType } from '../../Models/ProductTypes';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { IOrderInsert } from '../../Models/InsertModels/IOrderInsert';
import { CookieService } from 'ngx-cookie-service';
import { IOrder } from '../../Models/DisplayModels/IOrder';
import { OrderService } from '../../Services/order.service';
import { IOrderProductsInsert } from '../../Models/InsertModels/IOrderProductsInsert';
import { IOrderUpdate } from '../../Models/UpdateModels/IOrderUpdate';
import { SubjectService } from '../../Services/subject.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit, OnDestroy {

  proSub:any;

  arSub:any;

  prosSub:any;

  oSub:any;

  orSub:any;

  opSub:any;

  orderSub:any;

  product:any;

  products:IProduct[];

  rating:any;

  q:string;

  array:number[];

  key:boolean;

  constructor(private productService:GenericService<IProduct>, 
    private activatedRoute:ActivatedRoute,
    private orderService:OrderService,
    private cookieService:CookieService,
    private cdr:ChangeDetectorRef,
    private subjectService:SubjectService
    ){
    this.productService.baseUrl = "product";
    this.products = [];
    this.q = '1';
    this.array = [1,2,3,4,5];
    this.key = false;
  }

  ngOnInit(): void {
    this.arSub = this.activatedRoute.params.subscribe({
      next: id => {
        this.proSub = this.productService.GetById(id['id']).subscribe({
          next: data => {
            this.product = data;
            this.q = "1";
            this.key = false;
            this.cdr.detectChanges();
            this.rating = parseFloat(this.product.rating);
            this.array = [1,2,3,4,5];
            this.key = true;
            this.cdr.detectChanges();
            this.prosSub = this.productService.GetAll().subscribe({
              next: data => {
                if (this.product) {
                  this.products = data.filter(el => el.type == this.product.type && el.id != this.product.id)
                }
                console.log(this.products);
              },
              error: error => console.log(error)
            })
          }, 
          error: error => console.log(error)
        });
      },
      error: error => console.log(error)
    })

  }

  ngOnDestroy(): void {
    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }

    if (this.arSub != undefined) {
      this.arSub.unsubscribe();
    }

    if (this.prosSub != undefined) {
      this.prosSub.unsubscribe();
    }

    if (this.oSub != undefined) {
      this.oSub.unsubscribe();
    }

    if (this.orSub != undefined) {
      this.orSub.unsubscribe();
    }

    if (this.opSub != undefined) {
      this.opSub.unsubscribe();
    }

    if (this.orderSub != undefined) {
      this.orderSub.unsubscribe();
    }
  }

  getType(type:number){
    return ProductType[type];
  }

  reduce(){
    if (Number(this.q) - 1 == 0) {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "You can't set the quantity to less than one!",
      })
    }else{
      this.q = (Number(this.q) - 1).toString(); 
    }
  }

  increase(){
    if (Number(this.q) + 1 == (this.product.quantity + 1)) {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "You can't set the quantity to more than the available quantity!",
      })
    }else{
      this.q = (Number(this.q) + 1).toString();
    }
  }

  addToCart(){
    let order:IOrder;
    let uid:string = "";
    if (localStorage.getItem("RememberMe") === "true") {
      uid = localStorage.getItem("UserId") || "";
    }else{
      uid = this.cookieService.get("UserId");
    }
    this.orSub = this.orderService.GetAll().subscribe({
      next: data => {
        order = data.filter(el => el.userId == uid && el.status == 'O')[0];
        if (!order) {
          let obj:IOrderInsert = {
            totalPrice:this.product.price * Number(this.q),
            userId: uid
          } 
          this.oSub = this.orderService.addOrder(obj).subscribe({
            next: data => {
              console.log(data);
              order = data;

              let object:IOrderProductsInsert ={
                orderId:order.id,
                productId:this.product.id,
                quantity:Number(this.q),
                totalPrice:this.product.price * Number(this.q)
              } 
      
              this.opSub = this.orderService.addOrderProduct(object).subscribe({
                next: data => {
                  console.log(data);
                  Swal.fire({
                    title: "Added!",
                    text: "Product has been added to cart successfully!.",
                    icon: "success"
                  });
                  this.subjectService.increaseValue();
                },
                error: error => {
                  console.log(error);
                }
              })
            },
            error: error => {
              console.log(error);
            }
          })
        }else{
          let ob:IOrderUpdate = {
            id: order.id,
            status:'O',
            totalPrice: order.totalPrice + (this.product.price * Number(this.q))
          } 
          this.orderSub = this.orderService.Edit(order.id, ob).subscribe({
            next: data => {
              console.log(data);

              let object:IOrderProductsInsert ={
                orderId:order.id,
                productId:this.product.id,
                quantity:Number(this.q),
                totalPrice: this.product.price * Number(this.q)
              } 
      
              this.opSub = this.orderService.addOrderProduct(object).subscribe({
                next: data => {
                  console.log(data);
                  Swal.fire({
                    title: "Added!",
                    text: "Product has been added to cart successfully!.",
                    icon: "success"
                  });
                  this.subjectService.increaseValue();
                },
                error: error => {
                  console.log(error);
                }
              })
            },
            error: error => {
              console.log(error);
            }
          })
        }        
      },
      error: error => {
        console.log(error);
        let obj:IOrderInsert = {
          totalPrice:this.product.price * Number(this.q),
          userId: uid
        } 
        this.oSub = this.orderService.addOrder(obj).subscribe({
          next: data => {
            console.log(data);
            order = data;
            console.log("order Id", data);
            let object:IOrderProductsInsert ={
              orderId:order.id,
              productId:this.product.id,
              quantity:Number(this.q),
              totalPrice:this.product.price * Number(this.q)
            } 
    
            this.opSub = this.orderService.addOrderProduct(object).subscribe({
              next: data => {
                console.log(data);
                Swal.fire({
                  title: "Added!",
                  text: "Product has been added to cart successfully!.",
                  icon: "success"
                });
                this.subjectService.increaseValue();
              },
              error: error => {
                console.log(error);
              }
            })
          },
          error: error => {
            console.log(error);
          }
        })
        
      }
    })
  }
}
