import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { PaymentService } from '../payment.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.css'
})
export class PaymentsComponent {
  isPaying = false;
  errorMessage = '';
  orderId = '';

  constructor(
    private service: PaymentService,
    private route: ActivatedRoute
  ) {
    this.orderId = this.route.snapshot.paramMap.get('id') ?? '';
  }

  payWithCard(): void {
    if (this.isPaying) return;

    if (!this.orderId) {
      this.errorMessage = 'Missing orderId in URL.';
      return;
    }

    this.errorMessage = '';
    this.isPaying = true;

    this.service
      .createBankPaymentRequest(this.orderId)
      .pipe(finalize(() => (this.isPaying = false)))
      .subscribe({
        next: (res) => {
          if (!res?.paymentUrl) {
            this.errorMessage = 'Missing paymentUrl in response.';
            return;
          }
          window.location.href = res.paymentUrl;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Payment request failed. Please try again.';
        }
      });
  }

  payWithQR(): void {
    if (this.isPaying) return;

    if (!this.orderId) {
      this.errorMessage = 'Missing orderId in URL.';
      return;
    }

    this.errorMessage = '';
    this.isPaying = true;

    this.service
      .createBankPaymentQRRequest(this.orderId)
      .pipe(finalize(() => (this.isPaying = false)))
      .subscribe({
        next: (res) => {
          if (!res?.paymentUrl) {
            this.errorMessage = 'Missing paymentUrl in response.';
            return;
          }
          window.location.href = res.paymentUrl;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Payment request failed. Please try again.';
        }
      });
  }
}