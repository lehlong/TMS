import { Routes } from '@angular/router'
import { CurrencyComponent } from './currency/currency.component'
import { UnitComponent } from './unit/unit.component'
import { LocalComponent } from './local/local.component'
import { AreaComponent } from './area/area.component'
import { OpinionTypeComponent } from './opinion-type/opinion-type.component'
import { PeriodTimeComponent } from './period-time/period-time.component'
import { AuditPeriodComponent } from './audit-period/audit-period.component'
import { MgListTablesComponent } from './mg-list-tables/mg-list-tables.component'
import { MgOpinionListComponent } from './mg-opinion-list/mg-opinion-list.component'
import { AccountTypeComponent } from './account-type/account-type.component'
import { TemplateListTablesComponent } from './template-list-tables/template-list-tables/template-list-tables.component'
import { ListAuditComponent } from './list-audit/list-audit.component'
import { ListAuditEditComponent } from './list-audit/list-audit-edit/list-audit-edit/list-audit-edit.component'
import { TemplateListTablesPreviewComponent } from './template-list-tables-preview/template-list-tables-preview.component'
import { PreparingTemplateListTableComponent } from './preparing-template-list-table/preparing-template-list-table.component'
import { AuditPeriodListTablesComponent } from './audit-period-list-tables/audit-period-list-tables.component'
import { ReportComponent } from '../report/report/report.component'
import { MgListTablesGroupsComponent } from './mg-list-tables-groups/mg-list-tables-groups.component'
import { TemplateListTablesGroupsComponent } from './template-list-tables-groups/template-list-tables-groups.component'
import { TypeOfGoodsComponent } from './type-of-goods/type-of-goods.component'
import { MarketComponent } from './market/market.component'
import { SalesMethodComponent } from './sales-method/sales-method.component'
import { RetailPriceComponent } from './retail-price/retail-price.component'
import { GoodsComponent } from './goods/goods.component'
import { LaiGopDieuTietComponent } from './lai-gop-dieu-tiet/lai-gop-dieu-tiet.component'
import { CustomerComponent } from './customer/customer.component'
import { DeliveryPointComponent } from './delivery-point/delivery-point.component'
import { GiaGiaoTapDoanComponent } from './gia-giao-tap-doan/gia-giao-tap-doan.component'
import { MucGiamPhoThongComponent } from './muc-giam-pho-thong/muc-giam-pho-thong.component'
import { TinhToanDauRaComponent } from './tinh-toan-dau-ra/tinh-toan-dau-ra.component'
import { CustomerTypeComponent } from './customer-type/customer-type.component'
import { WarehouseComponent } from './warehouse/warehouse.component'
import { CompetitorComponent } from './competitor/competitor.component'
import { MapPointCustomerGoodsComponent } from './map-point-customer-goods/map-point-customer-goods.component'
import { MarketCompetitorComponent } from './market-competitor/market-competitor.component'
export const masterDataRoutes: Routes = [
  { path: 'currency', component: CurrencyComponent },
  { path: 'unit', component: UnitComponent },
  { path: 'local', component: LocalComponent },
  { path: 'area', component: AreaComponent },
  { path: 'opinion-type', component: OpinionTypeComponent },
  { path: 'audit-year', component: PeriodTimeComponent },
  { path: 'sales-method', component: SalesMethodComponent },
  { path: 'template-list-tables/:code', component: TemplateListTablesComponent },
  { path: 'audit-period', component: AuditPeriodComponent },
  { path: 'mg-list-tables/:code', component: MgListTablesComponent },
  { path: 'mg-opinion-list', component: MgOpinionListComponent },
  { path: 'account-type', component: AccountTypeComponent },
  { path: 'list-audit', component: ListAuditComponent },
  { path: 'mg-list-tables-groups', component: MgListTablesGroupsComponent },
  { path: 'template-list-tables-groups', component: TemplateListTablesGroupsComponent },
  { path: 'list-audit-edit/:code', component: ListAuditEditComponent },
  {
    path: 'template-list-tables-preview/:code',
    component: TemplateListTablesPreviewComponent,
  },
  {
    path: 'list-audit-edit/preparing-template-list-table/:code',
    component: PreparingTemplateListTableComponent,
  },
  {
    path: 'audit-period-list-tables',
    component: AuditPeriodListTablesComponent,
  },
  {path: 'report', component: ReportComponent,},
  {path: 'goods', component: GoodsComponent,},
  {path: 'market', component: MarketComponent,},
  {path: 'retail-price', component: RetailPriceComponent,},
  {path: 'lai-gop-dieu-tiet', component: LaiGopDieuTietComponent},
  {path: 'customer', component: CustomerComponent},
  {path: 'delivery-point', component: DeliveryPointComponent},
  {path: 'gia-giao-tap-doan', component: GiaGiaoTapDoanComponent},
  {path: 'muc-giam-pho-thong', component: MucGiamPhoThongComponent},
  {path: 'tinh-toan-dau-ra', component: TinhToanDauRaComponent},
  {path: 'customer-type', component: CustomerTypeComponent},
  {path: 'warehouse', component: WarehouseComponent},
  {path: 'competitor', component: CompetitorComponent},
  {path: 'map-point-customer-goods', component: MapPointCustomerGoodsComponent},
  {path: 'market-competitor', component: MarketCompetitorComponent},

]
