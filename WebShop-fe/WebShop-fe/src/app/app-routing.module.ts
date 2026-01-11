import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './layout/home/home.component';
import { ViewAllVehiclesComponent } from './vehicle/view-all-vehicles/view-all-vehicles.component';
import { ViewOneVehicleComponent } from './vehicle/view-one-vehicle/view-one-vehicle.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ViewReservationsByUserComponent } from './vehicle/view-reservations-by-user/view-reservations-by-user.component';
import { SuccessComponent } from './layout/success/success.component';
import { ErrorComponent } from './layout/error/error.component';
import { FailComponent } from './layout/fail/fail.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "vehicles", component: ViewAllVehiclesComponent },
  { path: 'vehicles/:id', component: ViewOneVehicleComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'reservations', component: ViewReservationsByUserComponent },
  { path: 'success', component: SuccessComponent },
  { path: 'error', component: ErrorComponent },
  { path: 'fail', component: FailComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
