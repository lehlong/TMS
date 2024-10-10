import { Routes } from '@angular/router'
import { OpinionListComponent } from './opinion-list/opinion-list.component'
import { ListTablesComponent } from './list-tables/list-tables.component'

export const businessRoutes: Routes = [
  { path: 'opinion-list/:code', component: OpinionListComponent },
  { path: 'list-tables/:groupCode/:code', component: ListTablesComponent },
]
