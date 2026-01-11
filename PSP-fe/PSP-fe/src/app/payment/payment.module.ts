import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentsComponent } from './payments/payments.component';
import { SuccessComponent } from './success/success.component';
import { ErrorComponent } from './error/error.component';
import { MerchantRegistrationComponent } from './merchant-registration/merchant-registration.component';



@NgModule({
  declarations: [
    PaymentsComponent,
    SuccessComponent,
    ErrorComponent,
  ],
  imports: [
    CommonModule
  ]
})
export class PaymentModule { }
