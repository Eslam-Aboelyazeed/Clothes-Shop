import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IProductUpdate } from '../../Models/UpdateModels/IProductUpdate';
import { GenericService } from '../../Services/generic.service';
import { Router } from '@angular/router';
import { ProductType } from '../../Models/ProductTypes';
import Swal from 'sweetalert2';
import { ProductService } from '../../Services/product.service';
import { IProduct } from '../../Models/DisplayModels/IProduct';

@Component({
  selector: 'app-update-product',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './update-product.component.html',
  styleUrl: './update-product.component.css'
})
export class UpdateProductComponent {
  proTypes:any;

  options:string[];

  form:FormGroup;
  
  proSub:any;

  pSub:any;

  @Input() productId:number;

  selectedFile: File | null = null;

  formData:FormData;

  @Output() updatingProductEvent:any;

  iUrl:string;

  imagePreview: string | ArrayBuffer | null;

  constructor(private productService:GenericService<IProduct>, private proService:ProductService ,private router:Router){
    this.form = new FormGroup({
      id:new FormControl(null),
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

    this.productService.baseUrl = "Product";
    
    this.productId = 0;

    this.formData = new FormData();

    this.updatingProductEvent = new EventEmitter();

    this.iUrl = '';

    this.imagePreview = null;
  }

  ngOnInit(): void {

    if (this.productId == undefined || this.productId == 0) {
      this.router.navigate(['/admindashboard']);
    }

    this.pSub = this.productService.GetById(this.productId).subscribe({
      next: data =>{
        if(data != undefined){
          this.getId.setValue(data.id);
          this.getName.setValue(data.name);
          this.getType.setValue(data.type);
          this.getDescription.setValue(data.description);
          this.getPrice.setValue(data.price);
          this.getQuantity.setValue(data.quantity);
          this.getRating.setValue(data.rating);
          this.iUrl = data.imageUrl;
        }
      },
      error:error => console.log(error)
    });
  }

  ngOnDestroy(): void {
    if (this.proSub != undefined) {
      this.proSub.unsubscribe();
    }
    if (this.pSub != undefined) {
      this.pSub.unsubscribe();
    }
  }

  get getId(){
    return this.form.controls['id'];
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

  onFileSelected(event:Event){
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.previewFile(this.selectedFile);
    }
  }

  previewFile(file: File): void {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.onerror = error => {
      console.error('Error reading file:', error);
    };
  }

  UpdateProduct(input:HTMLInputElement){
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
    }

    this.formData.append('productstring', JSON.stringify(this.form.value));
    
    this.proSub = this.proService.updateproduct(this.productId,this.formData).subscribe({
      next: data => {
        Swal.fire({
          title: "Updated!",
          text: "Product has been updated successfully!.",
          icon: "success"
        });

        input.value = '';
        this.updatingProductEvent.emit(data);
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
  }
}
