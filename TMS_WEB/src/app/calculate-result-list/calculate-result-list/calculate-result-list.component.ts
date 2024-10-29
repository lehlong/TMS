import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { CALCULATE_RESULT_LIST_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { CalculateResultListFilter } from '../../models/calculate-result-list/calculate-result-list.model'
import { CalculateResultListService } from '../../services/calculate-result-list/calculate-result-list.service'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './calculate-result-list.component.html',
  styleUrl: './calculate-result-list.component.scss',
})
export class CalculateResultListComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    name: ['', [Validators.required]],
    status: [true],
    fDate: [''],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new CalculateResultListFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  lst: any[] = []
  CALCULATE_RESULT_LIST_RIGHTS = CALCULATE_RESULT_LIST_RIGHTS

  constructor(
    private _service: CalculateResultListService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách loại hàng hoá',
        path: 'master-data/calculate-result-list',
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

  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  search() {
    this.isSubmit = false
    this._service.getall().subscribe({
      next: (data) => {
        this.lst = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }



  exportExcel() {
    return this._service
      .exportExcelCalculateResultList(this.filter)
      .subscribe((result: Blob) => {
        const blob = new Blob([result], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        })
        const url = window.URL.createObjectURL(blob)
        var anchor = document.createElement('a')
        anchor.download = 'danh-sach-dia-phuong.xlsx'
        anchor.href = url
        anchor.click()
      })
  }
  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some((local: any) => local.code === code)
  }
  submitForm(): void {
    this.isSubmit = true
    if (this.validateForm.valid) {
      const formData = this.validateForm.getRawValue()
      if (this.edit) {
        this._service.updateCalculateResultList(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      } else {
        if (this.isCodeExist(formData.code)) {
          this.message.error(
            `Mã khu vục ${formData.code} đã tồn tại, vui lòng nhập lại`,
          )
          return
        }
        this._service.createCalculateResultList(formData).subscribe({
          next: (data) => {
            this.search()
          },
          error: (response) => {
            console.log(response)
          },
        })
      }
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
    this.filter = new CalculateResultListFilter()
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
    this._service.deleteCalculateResultList(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  openEdit(data: any) {
    this.validateForm.setValue({
      code: data.code,
      name: data.name,
      fDate: data.fDate,
      status: data.status,
      isActive: data.isActive,
    })
    setTimeout(() => {
      this.edit = true
      this.visible = true
    }, 200)
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
}
