import { GenericService } from './../../Services/generic.service';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { IProductInsert } from '../../Models/InsertModels/IProductInsert';
import { ProductType } from '../../Models/ProductTypes';
import { HttpHeaders } from '@angular/common/http';
import { ProductService } from '../../Services/product.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent implements OnInit, OnDestroy {

  selectedFile: File | null = null;
  
  proTypes:any;

  options:string[];

  form:FormGroup;
  
  proSub:any;

  formData:FormData;

  @Output() addingProductEvent:any;

  constructor(private productService:ProductService, private router:Router){
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3)]),
      type: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      price: new FormControl(null, [Validators.required, Validators.min(1)]),
      quantity: new FormControl(null, [Validators.required]),
      rating: new FormControl(null)
    })

    this.proTypes = ProductType;

    this.options = Object.keys(this.proTypes).map(key => this.proTypes[key]);

    this.options = this.options.filter(el => typeof(el) != "number" )

    console.log(this.options);

    this.formData = new FormData();

    this.addingProductEvent = new EventEmitter();
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }
  }

  get getName(){
    return this.form.controls['name'];
  }

  get getType(){
    return this.form.controls['type'];
  }
  
  get getDescription(){
    return this.form.controls['description'];
  }

  get getPrice(){
    return this.form.controls['price'];
  }

  get getQuantity(){
    return this.form.controls['quantity'];
  }

  get getRating(){
    return this.form.controls['rating'];
  }

  get getImgUrl(){
    return this.form.controls['imageUrl'];
  }

  onFileSelected(event:Event){
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  AddProduct(input: HTMLInputElement){
    if (this.form.status == 'INVALID') {
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Please Input All the Required Fields with Valid Values",
      })
      return;
    }
    console.log(this.form.value);
    
    this.getType.setValue(Number(this.getType.value));

    if (this.selectedFile) {
      this.formData.append('imageUrl', this.selectedFile, this.selectedFile.name);
      this.formData.append('productstring', JSON.stringify(this.form.value));
      
      this.proSub = this.productService.addproduct(this.formData).subscribe({
        next: data => {
          console.log(data);
          Swal.fire({
            title: "Added!",
            text: "Product has been added successfully!.",
            icon: "success"
          });
          this.form.reset({
            name: new FormControl('', [Validators.required, Validators.minLength(3)]),
            type: new FormControl('', [Validators.required]),
            description: new FormControl(''),
            price: new FormControl(null, [Validators.required, Validators.min(1)]),
            quantity: new FormControl(null, [Validators.required]),
            rating: new FormControl(null)
          });
          input.value = '';
          this.addingProductEvent.emit(data);
        },
        error: error => {
          console.log(error);
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!"
          });
        }
      })
    }else{
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Error Uploading Image, Please try again later!",
      })
    }

  }
}
