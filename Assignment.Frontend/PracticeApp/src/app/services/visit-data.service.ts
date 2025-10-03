import { Injectable } from '@angular/core';
import { Visit } from '../models/visit.model';

@Injectable({
  providedIn: 'root'
})
export class VisitDataService {
  private visit: Visit | undefined = undefined;

  setVisit(visit: Visit): void {
    this.visit = visit;
  }

  getVisit(): Visit | undefined {
    return this.visit;
  }

  clearVisit(): void {
    this.visit = undefined;
  }
}