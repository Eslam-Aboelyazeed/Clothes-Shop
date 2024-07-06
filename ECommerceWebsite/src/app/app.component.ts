import { NavBarComponent } from './Components/nav-bar/nav-bar.component';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { FooterComponent } from './Components/footer/footer.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { HomeComponent } from './Components/home/home.component';
import { ProductListComponent } from './Components/product-list/product-list.component';
import { RegisterComponent } from './Components/register/register.component';
import { LoginComponent } from './Components/login/login.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { CartComponent } from './Components/cart/cart.component';
import { AdminDashBoardComponent } from './Components/admin-dash-board/admin-dash-board.component';
import { AddProductComponent } from './Components/add-product/add-product.component';
import { ContactUsComponent } from './Components/contact-us/contact-us.component';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from './Services/account.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, 
            NavBarComponent, 
            FooterComponent, 
            NotFoundComponent, 
            HomeComponent, 
            ProductListComponent, 
            RegisterComponent,
            LoginComponent, 
            ProductDetailsComponent,
            CartComponent,
            AdminDashBoardComponent,
            AddProductComponent,
            ContactUsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  // title = 'ECommerceWebsite';

  ActivePage:string;

  aSub:any;

  rSub:any;

  ChangeActivePage(activePage:string){
    this.ActivePage = activePage;
    console.log(this.ActivePage);
  }

  constructor(private cookieService:CookieService, private accountService:AccountService, private router: Router){
    this.ActivePage = '/';
  }

  ngOnInit(): void {
    if (localStorage.getItem("RememberMe") == "true") {
      if (!(this.cookieService.get("Token"))) {
        let obj = {
          email:localStorage.getItem("Email")||"",
          password:localStorage.getItem("Password")||"",
          isPersistent:true
        }
        this.aSub = this.accountService.login(obj).subscribe({
          next: data => {
              this.cookieService.set("Token",data.token, 1);
          },
          error: error => {
            console.log(error);
          }
        })
      }
    }

    this.rSub = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.ChangeActivePage(event.urlAfterRedirects);
      }
    })

    if (localStorage.getItem("RememberMe") == "true") {
      if(localStorage.getItem("Role") == "Admin"){
        this.router.navigate(['/admindashboard']);
      }
    }else{
      if(this.cookieService.get("Role") == "Admin"){
        this.router.navigate(['/admindashboard']);
      }
    }
  }

  ngOnDestroy(): void {
    if (this.aSub != undefined) {
      this.aSub.unsubscribe();
    }

    if (this.rSub != undefined) {
      this.rSub.unsubscribe();
    }
  }

}
