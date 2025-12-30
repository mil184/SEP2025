import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './layout/home/home.component';
import { ViewAllVehiclesComponent } from './vehicle/view-all-vehicles/view-all-vehicles.component';
import { ViewOneVehicleComponent } from './vehicle/view-one-vehicle/view-one-vehicle.component';
import { LoginComponent } from './auth/login/login.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "vehicles", component: ViewAllVehiclesComponent },
  { path: 'vehicles/:id', component: ViewOneVehicleComponent },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
