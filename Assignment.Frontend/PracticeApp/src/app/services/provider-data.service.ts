import { Injectable } from '@angular/core';
import { Provider } from '../models/provider.model';

@Injectable({
  providedIn: 'root'
})
export class ProviderDataService {
  private provider: Provider | undefined;

  setProvider(provider: Provider): void {
    this.provider = provider;
  }

  getProvider(): Provider | undefined {
    return this.provider;
  }

  clearProvider(): void {
    this.provider = undefined;
  }
}
