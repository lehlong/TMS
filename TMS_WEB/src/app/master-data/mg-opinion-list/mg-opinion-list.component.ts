import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { MgOpinionListFilter } from '../../models/master-data/mg-opinion-list.model'
import { AuditPeriodFilter } from '../../models/master-data/audit-period.model'
import { PeriodTimeFilter } from '../../models/master-data/period-time.model'
import { PaginationResult } from '../../models/base.model'
import { MGOPINIONLIST_RIGHTS } from '../../shared/constants'
import { MgOpinionListService } from '../../services/master-data/mg-opinion-list.service'
import { GlobalService } from '../../services/global.service'
import { PeriodTimeService } from '../../services/master-data/period-time.service'
import { AuditPeriodService } from '../../services/master-data/audit-period.service'
import { ActivatedRoute, Router } from '@angular/router'
import { NzMessageService } from 'ng-zorro-antd/message'

@Component({
  selector: 'app-mg-opinion-list',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './mg-opinion-list.component.html',
  styleUrl: './mg-opinion-list.component.scss',
})
export class MgOpinionListComponent {
  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    name: ['', [Validators.required]],
    description: [''],
    timeYear: ['', [Validators.required]],
    auditPeriod: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new MgOpinionListFilter()
  auditPeriodfilter = new AuditPeriodFilter()
  periodTimefilter = new PeriodTimeFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  MGOPIONIONLIST_RIGHT = MGOPINIONLIST_RIGHTS
  timeyear: any[] = []
  auditPeriod: any[] = []
  availableYears: any[] = [];
  availablePeriods: any[] = [];
  constructor(
    private _service: MgOpinionListService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private _ps: PeriodTimeService,
    private _ap: AuditPeriodService,
    private message: NzMessageService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách cây kiến nghị',
        path: 'master-data/mg-opinion-list',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }
  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search()
    this.loadYearsAndPeriods();

  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }
  loadYearsAndPeriods() {
    this._ps.getall().subscribe(years => {
      this.timeyear = years;
      this.updateAvailableYears();
    });

    this._ap.getall().subscribe(periods => {
      this.auditPeriod = periods;
    });
  }
  updateAvailableYears() {
    this._service.searchMgOpinionList(new MgOpinionListFilter()).subscribe(result => {
      const usedYearPeriods = result.data.map((item: { timeYear: any; auditPeriod: any }) => ({ year: item.timeYear, period: item.auditPeriod }));

      this.availableYears = this.timeyear.filter(year => {
        const periodsForYear = usedYearPeriods.filter((up: { year: any }) => up.year === year.timeyear);
        return periodsForYear.length < 2; // Year is available if it has less than 2 periods used
      });

      if (this.validateForm.get('timeYear')?.value) {
        this.updateAvailablePeriods();
      }
    });
  }

  updateAvailablePeriods() {
    const selectedYear = this.validateForm.get('timeYear')?.value;
    this._service.searchMgOpinionList(new MgOpinionListFilter()).subscribe(result => {
      const usedPeriods = result.data
        .filter((item: { timeYear: any }) => item.timeYear === selectedYear)
        .map((item: { auditPeriod: any }) => item.auditPeriod);

      this.availablePeriods = this.auditPeriod.filter(period => !usedPeriods.includes(period.code));
    });
  }
  onYearChange() {
    this.validateForm.patchValue({ auditPeriod: null });
    this.updateAvailablePeriods();
  }

  search() {
    this.isSubmit = false
    this._service.searchMgOpinionList(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  exportExcel() {
    return this._service
      .exportExcelMgOpinionList(this.filter)
      .subscribe((result: Blob) => {
        const blob = new Blob([result], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        })
        const url = window.URL.createObjectURL(blob)
        var anchor = document.createElement('a')
        anchor.download = 'danh-sach-cay-kien-nghi.xlsx'
        anchor.href = url
        anchor.click()
      })
  }
  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some(
      (opinion: any) => opinion.code === code,
    )
  }
  submitForm(): void {
    this.isSubmit = true
    if (this.validateForm.valid) {
      const formData = this.validateForm.getRawValue()
      if (this.isCodeExist(formData.code)) {
        this.message.error(
          `Mã kiến nghị ${formData.code} đã tồn tại, vui lòng nhập lại`,
        )
        return
      }
      this._service
        .createMgOpinionList(this.validateForm.getRawValue())
        .subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty()
          control.updateValueAndValidity({ onlySelf: true })
        }
      })
    }
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new MgOpinionListFilter()
    this.search()
  }

  openCreate() {
    this.edit = false
    this.visible = true
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string | number) {
    this._service.deleteMgOpinionList(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  openEdit(data: { code: string }) {
    this._service.searchMgOpinionList({ KeyWord: data.code }).subscribe({
      next: (result) => {
        if (result.data && result.data.length > 0) {
          const mgOpinionListData = result.data[0]
          this.router.navigate(['/business/opinion-list', data.code], {
            state: { mgOpinionListData },
          })
        }
      },
      error: (error) => console.error(error),
    })
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }
  getNameAuditPeriod(auditPeriod: string) {
    if (!this.auditPeriod) {
      return null
    }
    return this.auditPeriod.find((x: { code: string }) => x.code == auditPeriod)
      ?.auditPeriod
  }
}
