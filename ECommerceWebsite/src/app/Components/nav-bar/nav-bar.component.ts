import { MatIconModule } from '@angular/material/icon';
import {MatBadgeModule} from '@angular/material/badge';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from '../../Services/account.service';
import Swal from 'sweetalert2';
import { OrderService } from '../../Services/order.service';
import { SubjectService } from '../../Services/subject.service';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink, MatIconModule, MatBadgeModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent implements OnInit, OnDestroy {

  ActiveLink:string = "home";

  @Output() ActiveLinkChanged:any = new EventEmitter();

  UserName:string;

  lSub:any;

  rSub:any;

  oSub:any;

  cartCount:number;

  uid:string;

  constructor(
    private cookieService:CookieService, 
    private router:Router, private accountService:AccountService, 
    private subjectService:SubjectService,
    private orderService:OrderService
  ){
    this.UserName = "";
    this.cartCount = 0;
    this.uid = '';
  }

  ngOnInit(): void {
    if (localStorage.getItem("RememberMe") === "true") {
      this.UserName = localStorage.getItem("UserName") || "";
      this.uid = localStorage.getItem("UserId") || "";
    }else{
      this.uid = this.cookieService.get("UserId");
      this.UserName = this.cookieService.get("UserName");
    }

    this.rSub = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.changeActiveLink(event.urlAfterRedirects);
      }
    })

    // this.subjectService.sync();
    if (this.uid != '') {      
      this.oSub = this.orderService.getOrderCount(this.uid).subscribe({
        next: data => {
          this.subjectService.setValue(data);
        },
        error: error => {
          console.log(error);
        }
      })
    }

    this.subjectService.getValue().subscribe({
      next: data => {
        this.cartCount = data;
      },
      error: error => {
        console.log(error);
      }
    })
  }
  
  ngOnDestroy(): void {
    if (this.lSub != undefined) {
      this.lSub.unsubscribe();
    }

    if (this.rSub != undefined) {
      this.rSub.unsubscribe();
    }

    if (this.oSub != undefined) {
      this.oSub.unsubscribe();
    }
  }

  changeActiveLink(link:string){
    this.ActiveLink = link;

    this.ActiveLinkChanged.emit(this.ActiveLink);
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
            this.changeActiveLink('home');
            this.cookieService.deleteAll();
            localStorage.clear();
            this.router.navigate(['/']);
            this.UserName = "";
          },
          error: error => console.log(error)
        });

      }
    });
  }

}
