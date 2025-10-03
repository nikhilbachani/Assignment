import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VisitsTableComponent } from '../visits/visits-table/visits-table.component';
import { PracticeApiService } from '../../services/practice-api.service';
import { Visit } from '../../models/visit.model';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, VisitsTableComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  scheduledVisits: Visit[] = [];
  visitSubscription$: Subscription | undefined;

  constructor(private practiceApiService: PracticeApiService) {}

  ngOnInit(): void {
    this.visitSubscription$ = this.practiceApiService
      .getVisitsByDate(new Date().toISOString().split('T')[0], undefined, true)
      .subscribe({
        next: (visits) => {
          this.scheduledVisits = visits;
          console.log(this.scheduledVisits);
        },
        error: (error) => {
          console.error('Error fetching visits:', error);
        },
      });
  }

  ngOnDestroy(): void {
    this.visitSubscription$?.unsubscribe();
  }
}
