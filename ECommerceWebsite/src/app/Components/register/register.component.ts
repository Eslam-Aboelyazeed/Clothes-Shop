import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IUserInsert } from '../../Models/InsertModels/IUserInsert';
import { GenericService } from '../../Services/generic.service';
import Swal from 'sweetalert2';
import { AccountService } from '../../Services/account.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  form:FormGroup;
  aSub:any;

  constructor(private accountService:AccountService){
    this.form = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      confirmPassword: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(11)]),
      address: new FormControl('', [Validators.required, Validators.minLength(3)])
    });
  }

  ngOnDestroy(): void {
    if (this.aSub != undefined) {
      this.aSub.unsubscribe();
    }
  }

  get getFirstName(){
    return this.form.controls['firstName'];
  }

  get getLastName(){
    return this.form.controls['lastName'];
  }

  get getEmail(){
    return this.form.controls['email'];
  }

  get getPassword(){
    return this.form.controls['password'];
  }

  get getConfirmPassword(){
    return this.form.controls['confirmPassword'];
  }

  get getPhoneNumber(){
    return this.form.controls['phoneNumber'];
  }

  get getAddress(){
    return this.form.controls['address'];
  }

  register(){
    if (this.form.status == 'INVALID') {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Please Input All the Required Fields with Valid Values",
      })
      return;
    }

    this.aSub = this.accountService.register(this.form.value).subscribe({
      next: data => {
        Swal.fire({
          icon: "success",
          title: "Registered",
          text: "Please Check Your Email!",
        })
      },
      error: error => {
        console.log(error);
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something Went Wrong! Please try again later"
        });
      }
    })
  }
}
