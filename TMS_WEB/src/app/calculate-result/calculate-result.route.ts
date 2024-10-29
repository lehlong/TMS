import { Routes } from '@angular/router'
import { CalculateResultComponent } from './calculate-result/calculate-result.component'

export const calculateResultRoutes: Routes = [
  { path: 'detail/:code', component: CalculateResultComponent },
]
