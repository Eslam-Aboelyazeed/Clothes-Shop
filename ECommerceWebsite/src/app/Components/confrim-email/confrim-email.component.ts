import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { GenericService } from '../../Services/generic.service';
import { ConfirmEmailService } from '../../Services/confirm-email.service';

@Component({
  selector: 'app-confrim-email',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './confrim-email.component.html',
  styleUrl: './confrim-email.component.css'
})
export class ConfrimEmailComponent implements OnInit, OnDestroy {

  arSub:any;

  cSub:any;

  flag:string;

  constructor(private route:ActivatedRoute, private service:ConfirmEmailService){
    this.flag = "Loading...";
  }

  ngOnInit(): void {
    this.arSub = this.route.queryParams.subscribe({
      next: params => {
        console.log(decodeURIComponent(params['token']));
        this.cSub = this.service.confirmEmail(params['userId'],decodeURIComponent(params['token'])).subscribe({
          next: data => {
            console.log(data);
            if (data) {
              this.flag = "Successfully Confirmed Email"
            }else{
              this.flag = "Failed"
            }
          },
          error:error => console.log(error)  
        });
      },
      error: error => console.log(error)
    })
  }

  ngOnDestroy(): void {
    if (this.cSub != undefined) {
      this.cSub.unsubscribe();
    }

    if (this.arSub != undefined) {
      this.arSub.unsubscribe();
    }
  }
}
