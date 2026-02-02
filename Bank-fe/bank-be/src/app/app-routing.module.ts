import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payment/payments/payments.component';
import { QrPaymentsComponent } from './payment/qr-payments/qr-payments.component';

const routes: Routes = [
  { path: "payment/:id", component: PaymentsComponent },
  { path: "payment/qr/:id", component: QrPaymentsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
