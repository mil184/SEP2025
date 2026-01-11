import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { PaymentService } from '../payment.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.css'],
})
export class PaymentsComponent implements OnInit {
  orderId = '';
  isSubmitting = false;
  errorMessage = '';

  amount: number | null = null;
  isLoadingAmount = false;

  form = new FormGroup({
    pan: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    expiry: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    securityCode: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    cardholderName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
  });

  constructor(
    private route: ActivatedRoute,
    private paymentService: PaymentService
  ) {
    this.orderId = this.route.snapshot.paramMap.get('id') ?? '';
  }

  async ngOnInit(): Promise<void> {
    this.errorMessage = '';

    if (!this.orderId) {
      this.errorMessage = 'Missing orderId in URL.';
      return;
    }

    this.isLoadingAmount = true;
    try {
      const amount = await firstValueFrom(this.paymentService.amount(this.orderId));
      this.amount = amount;
    } catch (e: any) {
      this.errorMessage = e?.message ?? 'Failed to load payment amount.';
      this.amount = null;
    } finally {
      this.isLoadingAmount = false;
    }
  }

  private toExpirationDateIso(expiry: string): string {
    const cleaned = expiry.trim();
    const m = cleaned.match(/^(\d{2})\s*\/\s*(\d{2}|\d{4})$/);
    if (!m) throw new Error('Expiry must be in MM/YY format');

    const month = Number(m[1]);
    if (month < 1 || month > 12) throw new Error('Expiry month must be 01-12');

    const yy = m[2];
    const year = yy.length === 2 ? 2000 + Number(yy) : Number(yy);

    // last day of the month (common convention)
    const lastDay = new Date(year, month, 0);
    const yyyy = lastDay.getFullYear();
    const mm = String(lastDay.getMonth() + 1).padStart(2, '0');
    const dd = String(lastDay.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`; // "YYYY-MM-DD"
  }

  async onPayClick(): Promise<void> {
    this.errorMessage = '';

    if (!this.orderId) {
      this.errorMessage = 'Missing orderId in URL.';
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    this.isSubmitting = true;
    try {
      const pan = this.form.controls.pan.value.replace(/\s+/g, '');
      const expiry = this.form.controls.expiry.value;
      const securityCode = this.form.controls.securityCode.value;
      const cardholderName = this.form.controls.cardholderName.value;

      const expirationDate = this.toExpirationDateIso(expiry);

      const res = await firstValueFrom(
        this.paymentService.pay(this.orderId, {
          pan,
          securityCode,
          cardholderName,
          expirationDate,
        })
      );

      window.location.assign(res.redirectUrl);
    } catch (e: any) {
      this.errorMessage = e?.message ?? 'Payment failed.';
    } finally {
      this.isSubmitting = false;
    }
  }
}