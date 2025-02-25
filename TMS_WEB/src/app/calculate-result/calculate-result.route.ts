import { Routes } from '@angular/router'
import { CalculateResultComponent } from './calculate-result/calculate-result.component'
import { ViewWordComponent } from './view-word/view-word.component'

export const calculateResultRoutes: Routes = [
  { path: 'detail/:code', component: CalculateResultComponent },
  { path: 'view-word', component: ViewWordComponent },
]
