import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../vehicle.service';
import { Vehicle } from '../../model/vehicle.model';

@Component({
  selector: 'app-view-all-vehicles',
  templateUrl: './view-all-vehicles.component.html',
  styleUrl: './view-all-vehicles.component.scss'
})
export class ViewAllVehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  error: string | null = null;
  
  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.vehicleService.getAll().subscribe({
      next: (data) => {
        this.vehicles = data;
      },
      error: () => {
        this.error = 'Failed to load vehicles.';
      }
    });
  }
}
