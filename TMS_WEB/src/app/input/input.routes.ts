import { Routes } from '@angular/router'
import { VinhCuaLoComponent } from './vinh-cua-lo/vinh-cua-lo.component'
import { HeSoMatHangComponent } from './he-so-mat-hang/he-so-mat-hang.component'

export const inputRoutes: Routes = [
  { path: 'vinh-cua-lo', component: VinhCuaLoComponent },
  { path: 'he-so-mat-hang', component: HeSoMatHangComponent },

]
