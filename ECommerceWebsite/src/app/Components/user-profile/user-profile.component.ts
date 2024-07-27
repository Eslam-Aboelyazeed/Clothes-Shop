import { RouterLink } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from '../../Services/account.service';
import { OrderService } from '../../Services/order.service';
import { IUser } from '../../Models/DisplayModels/IUser';
import { IOrder } from '../../Models/DisplayModels/IOrder';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit, OnDestroy {

  active:string;

  uSub:any;

  oSub:any;

  user:IUser | undefined;

  userOrders: IOrder[];

  constructor(private cookieService:CookieService, private accountService:AccountService, private orderService:OrderService){
    this.active = "AM";
    this.userOrders = [];
  }

  ngOnInit(): void {
    let userName:string;
    let userId:string;
    if (localStorage.getItem("RememberMe") === "true") {
      userName = localStorage.getItem("UserName") || "";
      userId = localStorage.getItem("UserId") || "";
    }else{
      userName = this.cookieService.get("UserName");
      userId = this.cookieService.get("UserId");
    }

    this.uSub = this.accountService.getUserInfo(userName).subscribe({
      next: data => {
        this.user = data;
        this.oSub = this.orderService.getUserOrders(userId).subscribe({
          next: data => {
            console.log(data);
            this.userOrders = data;
          },
          error: error =>{
            console.log(error);
          }
        })
      },
      error:error => {
        console.log(error);
      }
    })

  }

  ngOnDestroy(): void {
    if (this.uSub != undefined) {
      this.uSub.unsubscribe();
    }

    if (this.oSub != undefined) {
      this.oSub.unsubscribe();
    }
  }

  changeActive(newActive:string){
    this.active = newActive;
  }
}
