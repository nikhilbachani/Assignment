import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PracticeApiService } from '../../../services/practice-api.service';
import { Observable, Subscription } from 'rxjs';
import { Provider } from '../../../models/provider.model';
import { Visit } from '../../..//models/visit.model';
import { VisitsTableComponent } from '../../visits/visits-table/visits-table.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-provider-view',
  templateUrl: './provider-view.component.html',
  styleUrls: ['./provider-view.component.scss'],
  standalone: true,
  imports: [CommonModule, VisitsTableComponent, FormsModule],
})
export class ProviderViewComponent implements OnInit {
  selectedProviderId: number | null = null;
  visits: Visit[] = [];
  showAlert: boolean = true;
  providers$!: Observable<Provider[]>;
  getVisitsSubscription$: Subscription | undefined;

  constructor(private practiceApiService: PracticeApiService) {}

  ngOnInit(): void {
    this.providers$ = this.practiceApiService.getProviders();
  }

  onProviderChange(providerId: string): void {
    this.selectedProviderId = Number(providerId);

    if (this.selectedProviderId) {
      this.getVisitsSubscription$ = this.practiceApiService
        .getVisitsByDate(
          new Date().toISOString().split('T')[0],
          this.selectedProviderId
        )
        .subscribe((visits) => {
          this.visits = visits; // Update visits data
        });
    }
  }

  ngOnDestroy(): void {
    this.getVisitsSubscription$?.unsubscribe();
  }
}
