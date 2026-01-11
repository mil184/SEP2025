import { Component, OnInit } from '@angular/core';
import { Vehicle } from '../../model/vehicle.model';
import { ActivatedRoute } from '@angular/router';
import { VehicleService } from '../vehicle.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';
import { ReservationService } from '../reservation.service';
import { Reservation } from '../../model/reservation.model';

@Component({
  selector: 'app-view-one-vehicle',
  templateUrl: './view-one-vehicle.component.html',
  styleUrl: './view-one-vehicle.component.scss'
})
export class ViewOneVehicleComponent implements OnInit {
  vehicle: Vehicle | null = null;
  isLoading = false;
  error: string | null = null;
  isLoggedIn = false;

  reservationForm = new FormGroup({
  startDate: new FormControl('', Validators.required),
  endDate: new FormControl('', Validators.required),
  });

  constructor(
    private route: ActivatedRoute,
    private vehicleService: VehicleService,
    private authService: AuthService,
    private reservationService: ReservationService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'Missing vehicle id in URL.';
      return;
    }

    this.isLoggedIn = this.authService.isLoggedIn();

    this.fetchVehicle(id);
  }

  private fetchVehicle(id: string): void {
    this.isLoading = true;
    this.error = null;
    this.vehicle = null;

    this.vehicleService.getById(id).subscribe({
      next: (res) => {
        this.vehicle = res;
        this.isLoading = false;
      },
      error: (err) => {
        const msg =
          err?.error?.message ||
          err?.message ||
          'Failed to load vehicle. Please try again.';

        this.error = msg;
        this.isLoading = false;
      }
    });
  }

  public reserve(): void {
    console.log(this.reservationForm);
    if (this.reservationForm.invalid) return;

    const startDate = this.reservationForm.value.startDate!;
    const endDate = this.reservationForm.value.endDate!;

    if (endDate <= startDate) {
      return;
    }

    const userId = this.authService.getUserId();
    if (userId == null){
      return;
    }

    const reservation: Reservation = {
      userId: userId,
      vehicleId: this.vehicle!.id,
      startDate: startDate,
      endDate: endDate
    };
    
    this.reservationService.create(reservation).subscribe({
      next: (created: { redirectUrl: string }) => {
        window.location.href = created.redirectUrl;
      },
      error: (err) => console.error(err),
    });
  }
}
