import { Component, OnInit } from '@angular/core';
import { Vehicle } from '../../model/vehicle.model';
import { ActivatedRoute } from '@angular/router';
import { VehicleService } from '../vehicle.service';

@Component({
  selector: 'app-view-one-vehicle',
  templateUrl: './view-one-vehicle.component.html',
  styleUrl: './view-one-vehicle.component.scss'
})
export class ViewOneVehicleComponent implements OnInit {
vehicle: Vehicle | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private vehicleService: VehicleService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.error = 'Missing vehicle id in URL.';
      return;
    }

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
        // handle backend messages nicely if available
        const msg =
          err?.error?.message ||
          err?.message ||
          'Failed to load vehicle. Please try again.';

        this.error = msg;
        this.isLoading = false;
      }
    });
  }
}
