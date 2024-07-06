import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from '../../Services/order.service';
import { IOrder } from '../../Models/DisplayModels/IOrder';
import { CookieService } from 'ngx-cookie-service';
import { ProductType } from '../../Models/ProductTypes';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { SubjectService } from '../../Services/subject.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit, OnDestroy {

  oSub:any;

  roSub:any;

  uoSub:any;

  rpSub:any;

  cSub:any;

  uSub:any;

  order:IOrder | undefined;

  tax:number;

  constructor(private orderService:OrderService, private cookieService:CookieService, private subjectService:SubjectService){
    this.tax = 0;
  }

  ngOnInit(): void {
    let uid;
    if (localStorage.getItem("RememberMe") === "true") {
      uid = localStorage.getItem("UserId") || "";
    }else{
      uid = this.cookieService.get("UserId");
    }
    this.oSub = this.orderService.GetAll().subscribe({
      next: data => {
        if (data) {          
          this.order = data.filter(el => el.userId == uid && el.status == 'O')[0];
          if (this.order) {
            this.order.orderProducts.forEach(el => this.tax += (10 * el.quantity));
          }
        }
      },
      error: error => {
        console.log(error);
      }
    })

    
  }

  ngOnDestroy(): void {
    if (this.oSub != undefined) {
      this.oSub.unsubscribe();
    }

    if (this.roSub != undefined) {
      this.roSub.unsubscribe();
    }

    if (this.uoSub != undefined) {
      this.uoSub.unsubscribe();
    }

    if (this.rpSub != undefined) {
      this.rpSub.unsubscribe();
    }

    if (this.cSub != undefined) {
      this.cSub.unsubscribe();
    }

    if (this.uSub != undefined) {
      this.uSub.unsubscribe();
    }
  }

  getType(type:number){
    return ProductType[type];
  }
  
  onQuantityChanged(pid:number){
    for (let i = 0; i < this.order?.orderProducts.length!!; i++) {
      if (this.order?.orderProducts[i].pId == pid) {
        this.order.orderProducts[i].totalPrice = this.order.orderProducts[i].quantity * this.order.orderProducts[i].productPrice;
        let newtotal = 0;
        this.tax = 0;
        this.order.orderProducts.forEach(el => {
          newtotal+= el.totalPrice;
          this.tax += (10 * el.quantity);
        } );
        this.order.totalPrice = newtotal;
      }
    }
  }

  removeProduct(pid:number){
    this.rpSub = this.orderService.Delete(this.order?.id!!, pid).subscribe({
      next: data => {
        console.log(data);
        
        Swal.fire({
          title: "Removed!",
          text: "Product has been removed from cart successfully!.",
          icon: "success"
        });

        this.subjectService.decreaseValue();

        if (this.order) {
          this.order.orderProducts = this.order?.orderProducts.filter(el => el.pId != pid);
          let newtotal = 0;
          this.tax = 0;
          this.order.orderProducts.forEach(el => {
            newtotal+= el.totalPrice;
            this.tax += (10 * el.quantity);
          } );
          this.order.totalPrice = newtotal;
        }

        if (this.order?.orderProducts.length == 0) {
          this.roSub = this.orderService.DeleteOrder(this.order.id).subscribe({
            next: data => {
              console.log(data);
              this.order = undefined;
            },
            error: error => {
              console.log(error);
            }
          })
        }else{
          this.uoSub = this.orderService.Edit(this.order?.id!!, {
            id: this.order?.id!!,
            status: 'O',
            totalPrice:this.order?.totalPrice!!
          }).subscribe({
            next: data => {
              console.log(data);
            },
            error: error => {
              console.log(error);
            }
          })
        }
      },
      error: error =>{
        console.log(error);
      }
    })
  }

  checkout(){
    this.cSub = this.orderService.Edit(this.order?.id!!, {
      id: this.order?.id!!, 
      status: 'D',
      totalPrice: this.order?.totalPrice!!
    }).subscribe({
      next: data => {
        console.log(data);
        this.uSub = this.orderService.updateOrderProducts(this.order?.id!!, this.order!!).subscribe({
          next: data => {
            console.log(data);
            this.order = undefined;

            Swal.fire({
              title: "Purchased!",
              text: "Products will be shipped to your address soon!.",
              icon: "success"
            });
    
            this.subjectService.resetValue();
          },
          error : error => {
            console.log(error);
          }
        })
      },
      error: error => {
        console.log(error);
      }
    })
  }
}
