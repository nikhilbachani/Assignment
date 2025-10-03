import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProviderViewComponent } from './components/provider/provider-view/provider-view.component';
import { VisitDetailsComponent } from './components/visits/visit-details/visit-details.component';
import { AddVisitComponent } from './components/visits/add-visit/add-visit.component';
import { PatientComponent } from './components/patient/patient-tabs/patient-tabs.component';
import { VisitPaymentComponent } from './components/visits/visit-payment/visit-payment.component';
import { VisitInvoiceComponent } from './components/visits/visit-invoice/visit-invoice.component';
import { VisitReceiptComponent } from './components/visits/visit-receipt/visit-receipt.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'provider-view', component: ProviderViewComponent },
  { path: 'visit-details', component: VisitDetailsComponent },
  { path: 'add-visit', component: AddVisitComponent },
  { path: 'visit-payment', component: VisitPaymentComponent },
  { path: 'visit-invoice', component: VisitInvoiceComponent },
  { path: 'visit-receipt', component: VisitReceiptComponent },
  { path: 'patient', component: PatientComponent },
];
