import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DebitsComponent } from './debits/debits.component';
import { CreditsComponent } from './credits/credits.component';
import { GraphsChartsComponent } from './graphs-charts/graphs-charts.component';
import { DataCurrentMonthComponent } from './data-current-month/data-current-month.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'debits', component: DebitsComponent },
  { path: 'credits', component: CreditsComponent },
  { path: 'graphs-charts', component: GraphsChartsComponent },
  { path: 'data-current-month', component: DataCurrentMonthComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];
