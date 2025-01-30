import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DebitsComponent } from './debits/debits.component';
import { CreditsComponent } from './credits/credits.component';
import { GraphsChartsComponent } from './graphs-charts/graphs-charts.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'debits', component: DebitsComponent },
  { path: 'credits', component: CreditsComponent },
  { path: 'graphs-charts', component: GraphsChartsComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];
