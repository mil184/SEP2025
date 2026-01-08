import { Component } from '@angular/core';
import { PaymentService } from '../payment.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.css'
})
export class PaymentsComponent {
  isPaying = false;
  errorMessage = '';

  constructor(private service: PaymentService) {}

  payWithCard(): void {

    
  }
}
