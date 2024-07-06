import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { OrderService } from './order.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class SubjectService implements OnInit, OnDestroy {

  subject:Subject<number>;

  index:number;

  private oSub:any;

  private uid:string;

  constructor(private orderService:OrderService, private cookieService: CookieService) {
    this.subject = new Subject();
    this.index = 0;
    this.uid = '';
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    if (this.oSub != undefined) {
      this.oSub.unsubscribe();
    }
  }

  sync(){
    if (localStorage.getItem("RememberMe") === "true") {
      this.uid = localStorage.getItem("UserId") || "";
    }else{
      this.uid = this.cookieService.get("UserId");
    }
    if (this.uid != '') {      
      this.oSub = this.orderService.getOrderCount(this.uid).subscribe({
        next: data => {
          this.index = data;
  
          this.subject.next(data);
        },
        error: error => {
          console.log(error);
        }
      })
    }
  }

  increaseValue(){
    this.subject.next(++this.index);
  }

  decreaseValue(){
    this.subject.next(--this.index);
  }

  resetValue(){
    this.index = 0;
    this.subject.next(this.index);
  }

  getValue(){
    return this.subject.asObservable();
  }
}
