import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfig } from '../configs/api-config';
import { Provider } from '../models/provider.model';
import { AddVisitRequest, AddVisitResponse, Visit } from '../models/visit.model';
import { AddPatientRequest, AddPatientResponse, Patient } from '../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PracticeApiService {
  private apiUrl = `${ApiConfig.baseUrl}`;

  constructor(private http: HttpClient) {}

  getProviders(): Observable<Provider[]> {
    return this.http.get<Provider[]>(`${this.apiUrl}/providers`);
  }

  getVisitsByDate(date: string, providerId?: number, includeInvoice?: boolean): Observable<Visit[]> {
    var requestUrl = `${this.apiUrl}/visits?visitDate=${date}`;
    if (providerId) {
      requestUrl += `&providerId=${providerId}`;
    }

    if (includeInvoice) {
      requestUrl += `&includeInvoice=${includeInvoice}`;
    }

    return this.http.get<Visit[]>(requestUrl);
  }

  addVisit(visit: AddVisitRequest): Observable<AddVisitResponse> {
    return this.http.post<AddVisitResponse>(`${this.apiUrl}/visits`, visit);
  }

  addPatient(patient: AddPatientRequest): Observable<AddPatientResponse> {
    return this.http.post<AddPatientResponse>(`${this.apiUrl}/patients`, patient);
  }

  searchPatients(searchTerm: string): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${this.apiUrl}/patients/search?searchTerm=${searchTerm}`);
  }

  getAvailableTimeSlots(providerId: number, date: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/visits/slots/?visitDate=${date}&providerId=${providerId}`);
  }

  payInvoice(invoiceId: number, paymentMethod: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/invoices/pay`, { invoiceId, paymentMethod });
  }
}