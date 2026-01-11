import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payment/payments/payments.component';
import { HttpClientModule } from '@angular/common/http';

const routes: Routes = [
  { path: "payment/:id", component: PaymentsComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    HttpClientModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
