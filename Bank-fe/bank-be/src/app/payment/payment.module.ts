import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentsComponent } from './payments/payments.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    PaymentsComponent
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    CommonModule
  ]
})
export class PaymentModule { }
