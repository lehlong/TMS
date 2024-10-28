import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { HE_SO_MAT_HANG_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { HeSoMatHangFilter } from '../../models/input/he-so-mat-hang.model'
import { HeSoMatHangService } from '../../services/input/he-so-mat-hang.service'
import { GoodsService } from '../../services/master-data/goods.service'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './he-so-mat-hang.component.html',
  styleUrl: './he-so-mat-hang.component.scss',
})
export class HeSoMatHangComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    goodsCode: ['', [Validators.required]],
    heSoVcf: ['', [Validators.required]],
    l15ChuaVatBvmt: ['', [Validators.required]],
    l15ChuaVatBvmtNbl: ['', [Validators.required]],
    thueBvmt: ['', [Validators.required]],
    fromDate: ['', [Validators.required]],
    toDate: ['', [Validators.required]],
    date: null,
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new HeSoMatHangFilter()
  paginationResult = new PaginationResult()
  goodsResult: any[] = []
  loading: boolean = false
  HE_SO_MAT_HANG_RIGHTS = HE_SO_MAT_HANG_RIGHTS
  constructor(
    private _service: HeSoMatHangService,
    private _goodsService: GoodsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Hệ số mặt hàng',
        path: 'input/he-so-mat-hang',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })

  }
  date: any = null

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search()
    this.getAllGoods()
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
    this._service.searchHeSoMatHang(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllGoods() {
    this.isSubmit = false
    this._goodsService.getall().subscribe({
      next: (data) => {
        this.goodsResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  onChangeDate(result: Date[]): void {
    if (result) {
      this.validateForm.patchValue({
        fromDate: new Date(result[0].setHours(result[0].getHours() + 7)),
        toDate: new Date(result[1].setHours(result[1].getHours() + 7)),
      })
    }else{
      return
    }
  }

  exportExcel() {
    return this._service
      .exportExcelHeSoMatHang(this.filter)
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
    console.log(this.validateForm.getRawValue());

    if (this.validateForm.valid) {
      const formData = this.validateForm.getRawValue()
      console.log(formData);

      if (this.edit) {
        this._service.updateHeSoMatHang(formData).subscribe({
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
        this._service.createHeSoMatHang(formData).subscribe({
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
    this.filter = new HeSoMatHangFilter()
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

  openEdit(data: any) {
    console.log(data);

    this.validateForm.setValue({
      code : data.code,
      goodsCode: data.goodsCode,
      heSoVcf: data.heSoVcf,
      thueBvmt: data.thueBvmt,
      l15ChuaVatBvmt: data.l15ChuaVatBvmt,
      l15ChuaVatBvmtNbl: data.l15ChuaVatBvmtNbl,
      fromDate: data.fromDate,
      toDate: data.toDate,
      isActive: data.isActive,
      date: ''
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
