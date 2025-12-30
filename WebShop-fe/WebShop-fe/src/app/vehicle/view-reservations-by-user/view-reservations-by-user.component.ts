import { Component } from '@angular/core';
import { ReservationService } from '../reservation.service';
import { Reservation } from '../../model/reservation.model';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-view-reservations-by-user',
  templateUrl: './view-reservations-by-user.component.html',
  styleUrl: './view-reservations-by-user.component.scss'
})
export class ViewReservationsByUserComponent {
  reservations: Reservation[] = [];
  error: string | null = null;
  userId!: string;

  constructor(private reservationService: ReservationService, private authService: AuthService) {}
  
  ngOnInit(): void {
    const userId = this.authService.getUserId();
    if (userId == null){
      return;
    }

    this.reservationService.getAllByUser(userId).subscribe({
      next: (data) => {
        this.reservations = data;
      },
      error: () => {
        this.error = 'Failed to load reservations.';
      }
    });
  }
}
