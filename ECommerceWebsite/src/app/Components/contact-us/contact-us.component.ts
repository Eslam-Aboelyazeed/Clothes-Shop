import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GenericService } from '../../Services/generic.service';
import { IContactUsInsert } from '../../Models/InsertModels/IContactUsInsert';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-contact-us',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './contact-us.component.html',
  styleUrl: './contact-us.component.css'
})
export class ContactUsComponent implements OnDestroy {

  cSub:any;

  form:FormGroup;

  constructor(private contactUsService:GenericService<IContactUsInsert>){
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      message: new FormControl('', [Validators.required, Validators.minLength(25)])
    })

    contactUsService.baseUrl = "ContactUs"
  }

  ngOnDestroy(): void {
    if (this.cSub != undefined) {
      this.cSub.unsubscribe();
    }
  }

  get getEmail(){
    return this.form.controls['email'];
  }

  get getMessage(){
    return this.form.controls['message'];
  }

  submitMessage(){

    if(this.form.status == 'INVALID'){
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Please Input All the Required Fields with Valid Values",
      })
      return;
    }

    this.cSub = this.contactUsService.Add(this.form.value).subscribe({
      next: data => {
        Swal.fire({
          title: "Message Sent!",
          text: "we will get back to you as soon as possible!",
          icon: "success"
        });
        this.getEmail.setValue('');
        this.getMessage.setValue('');
      },
      error: error => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong!"
        });
      }
    })
  }
}
