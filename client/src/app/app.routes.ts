import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DebitsComponent } from './debits/debits.component';
import { CreditsComponent } from './credits/credits.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'debits', component: DebitsComponent },
  { path: 'credits', component: CreditsComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];
