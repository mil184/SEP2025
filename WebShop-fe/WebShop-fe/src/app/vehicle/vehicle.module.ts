import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewOneVehicleComponent } from './view-one-vehicle/view-one-vehicle.component';
import { ViewAllVehiclesComponent } from './view-all-vehicles/view-all-vehicles.component';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from '../app-routing.module';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatToolbarModule} from '@angular/material/toolbar';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ViewReservationsByUserComponent } from './view-reservations-by-user/view-reservations-by-user.component';


@NgModule({
  declarations: [
    ViewOneVehicleComponent,
    ViewAllVehiclesComponent,
    ViewReservationsByUserComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    BrowserModule,
    AppRoutingModule,

    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatListModule,
    MatProgressSpinnerModule,
  ]
})
export class VehicleModule { }
