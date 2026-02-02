import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentsComponent } from './payments/payments.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { QrPaymentsComponent } from './qr-payments/qr-payments.component';


@NgModule({
  declarations: [
    PaymentsComponent,
    QrPaymentsComponent
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    CommonModule
  ]
})
export class PaymentModule { }
