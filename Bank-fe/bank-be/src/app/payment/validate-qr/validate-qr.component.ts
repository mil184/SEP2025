import { Component } from '@angular/core';
import { PaymentService } from '../payment.service';

@Component({
  selector: 'app-validate-qr',
  templateUrl: './validate-qr.component.html',
  styleUrls: ['./validate-qr.component.css']
})
export class ValidateQrComponent {
  selectedFile: File | null = null;

  loading = false;
  error: string | null = null;

  decodedText: string | null = null;

  valid: boolean | null = null;

  constructor(private paymentService: PaymentService) {}

  onFileSelected(event: Event) {
    this.error = null;
    this.decodedText = null;
    this.valid = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;

    if (!file) {
      this.selectedFile = null;
      return;
    }

    if (file.type !== 'image/png') {
      this.selectedFile = null;
      this.error = 'Please upload a PNG image.';
      return;
    }

    this.selectedFile = file;
  }

  validate() {
    if (!this.selectedFile) {
      this.error = 'Please choose a PNG file first.';
      return;
    }

    this.loading = true;
    this.error = null;
    this.decodedText = null;
    this.valid = null;

    this.paymentService.validateQr(this.selectedFile).subscribe({
      next: (res) => {
        this.loading = false;

        if (res) {
          this.valid = true;
          this.decodedText = res;
        } else {
          this.valid = false;
          this.decodedText = null;
        }
      },
      error: (err) => {
        this.loading = false;
        this.error =
          err?.error?.message ??
          err?.message ??
          'Request failed.';
      }
    });
  }
}