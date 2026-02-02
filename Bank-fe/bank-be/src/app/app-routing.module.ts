import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payment/payments/payments.component';
import { QrPaymentsComponent } from './payment/qr-payments/qr-payments.component';
import { ValidateQrComponent } from './payment/validate-qr/validate-qr.component';

const routes: Routes = [
  { path: "qr/validate", component: ValidateQrComponent },
  { path: "payment/:id", component: PaymentsComponent },
  { path: "payment/qr/:id", component: QrPaymentsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
