import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { ProductListComponent } from './Components/product-list/product-list.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { ContactUsComponent } from './Components/contact-us/contact-us.component';
import { AdminDashBoardComponent } from './Components/admin-dash-board/admin-dash-board.component';
import { UserProfileComponent } from './Components/user-profile/user-profile.component';
import { ConfrimEmailComponent } from './Components/confrim-email/confrim-email.component';
import { CartComponent } from './Components/cart/cart.component';
import { authenticationGuard } from './Guards/authentication.guard';
import { authorizationGuard } from './Guards/authorization.guard';
import { accountGuard } from './Guards/account.guard';
export const routes: Routes = [
    {path:'', component: HomeComponent},
    {path:'products', component: ProductListComponent, canActivate: [authenticationGuard]},
    {path:'products/:id', component: ProductDetailsComponent, canActivate: [authenticationGuard]},
    {path:'cart', component: CartComponent, canActivate: [authenticationGuard]},
    {path:'login', component: LoginComponent, canActivate:[accountGuard]},
    {path:'register', component: RegisterComponent, canActivate:[accountGuard]},
    {path:'contactus', component: ContactUsComponent, canActivate: [authenticationGuard]},
    {path:'admindashboard', component: AdminDashBoardComponent, canActivate: [authorizationGuard]},
    {path:'userdetails', component: UserProfileComponent, canActivate: [authenticationGuard]},
    {path:'confirm', component: ConfrimEmailComponent, data:{expectedQueryParams: ['userId', 'token']}},
    {path:'**', component: NotFoundComponent}
];
