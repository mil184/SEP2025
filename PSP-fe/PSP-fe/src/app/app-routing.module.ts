import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payment/payments/payments.component';
import { HttpClientModule } from '@angular/common/http';
import { MerchantRegistrationComponent } from './payment/merchant-registration/merchant-registration.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  { path: "payment/:id", component: PaymentsComponent },
  { path: "register-merchant", component: MerchantRegistrationComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
