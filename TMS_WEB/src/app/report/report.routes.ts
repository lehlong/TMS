import { Routes } from '@angular/router';
import { ReportComponent } from './report/report.component';
import { TemplateReportComponent } from './templates-report/template-report/template-report.component';

export const reportRoutes: Routes = [
    { path: 'report', component: ReportComponent },
    { path: 'template-report', component: TemplateReportComponent }
];
