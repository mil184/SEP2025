import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { PaymentService } from '../payment.service';
import { BrowserQRCodeReader } from '@zxing/browser';

@Component({
  selector: 'app-payments',
  templateUrl: './qr-payments.component.html',
  styleUrls: ['./qr-payments.component.css'],
})
export class QrPaymentsComponent implements OnInit, OnDestroy {
  orderId = '';
  isSubmitting = false;
  errorMessage = '';

  qrUrl: string | null = null;
  isLoadingQr = false;

  currency: string | null = null;
  amount: number | null = null;
  brojRacuna: string | null = null;
  ownerName: string | null = null;
  qrText: string | null = null;

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

    this.isLoadingQr = true;
    try {
      const blob = await firstValueFrom(this.paymentService.qr(this.orderId));

      if (this.qrUrl) URL.revokeObjectURL(this.qrUrl);

      this.qrUrl = URL.createObjectURL(blob);
    } catch (e: any) {
      this.errorMessage = e?.message ?? 'Failed to load QR code.';
      this.qrUrl = null;
    } finally {
      this.isLoadingQr = false;
    }
  }

  ngOnDestroy(): void {
    if (this.qrUrl) URL.revokeObjectURL(this.qrUrl);
  }

  private toExpirationDateIso(expiry: string): string {
    const cleaned = expiry.trim();
    const m = cleaned.match(/^(\d{2})\s*\/\s*(\d{2}|\d{4})$/);
    if (!m) throw new Error('Expiry must be in MM/YY format');

    const month = Number(m[1]);
    if (month < 1 || month > 12) throw new Error('Expiry month must be 01-12');

    const yy = m[2];
    const year = yy.length === 2 ? 2000 + Number(yy) : Number(yy);

    const lastDay = new Date(year, month, 0);
    const yyyy = lastDay.getFullYear();
    const mm = String(lastDay.getMonth() + 1).padStart(2, '0');
    const dd = String(lastDay.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  async scanQrFromImage(): Promise<void> {
    this.errorMessage = '';

    if (!this.qrUrl) {
      this.errorMessage = 'QR image is not loaded yet.';
      return;
    }

    try {
      const img = new Image();
      img.crossOrigin = 'anonymous';
      img.src = this.qrUrl;

      await new Promise<void>((resolve, reject) => {
        img.onload = () => resolve();
        img.onerror = () => reject(new Error('Failed to load QR image.'));
      });

      const reader = new BrowserQRCodeReader();
      const result = await reader.decodeFromImageElement(img);

      const text = result.getText();
      this.qrText = text;

      const parsed = this.parseQrPayload(text);

      this.currency = parsed.currency;
      this.amount = parsed.amount;
      this.brojRacuna = parsed.brojRacuna;
      this.ownerName = parsed.ownerName;
    } catch (e: any) {
      this.errorMessage = e?.message ?? 'Failed to scan QR code.';
    }
  }

  private parseQrPayload(payload: string): {
    currency: string | null;
    amount: number | null;
    brojRacuna: string | null;
    ownerName: string | null;
  } {
    const parts = payload.split('|');

    const map: Record<string, string> = {};
    for (const p of parts) {
      const idx = p.indexOf(':');
      if (idx === -1) continue;
      const key = p.slice(0, idx).trim();
      const val = p.slice(idx + 1).trim();
      map[key] = val;
    }

    const brojRacuna = map['R'] ?? null;

    const ownerName = map['N'] ?? null;

    const i = map['I'] ?? '';
    let currency: string | null = null;
    let amount: number | null = null;

    if (i.length >= 4) {
      currency = i.slice(0, 3);

      const amountStr = i.slice(3).trim();
      const normalized = amountStr.replace('.', '').replace(',', '.');
      const parsed = Number(normalized);
      amount = Number.isFinite(parsed) ? parsed : null;
    }

    return { currency, amount, brojRacuna, ownerName };
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