import { Component, Input } from '@angular/core';
import { IProduct } from '../../Models/DisplayModels/IProduct';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.css'
})
export class ProductItemComponent {
  
  @Input() product:IProduct | undefined
  
  constructor(){

  }
}
