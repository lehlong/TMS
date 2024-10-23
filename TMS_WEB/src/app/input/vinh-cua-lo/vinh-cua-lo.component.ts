import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { VINH_CUA_LO_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { VinhCuaLoFilter } from '../../models/input/vinh-cua-lo.model'
import { VinhCuaLoService } from '../../services/input/vinh-cua-lo.service'
import { GoodsComponent } from '../../master-data/goods/goods.component'
import { GoodsService } from '../../services/master-data/goods.service'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './vinh-cua-lo.component.html',
  styleUrl: './vinh-cua-lo.component.scss',
})
export class VinhCuaLoComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    goodsCode: ['', [Validators.required]],
    GblcsV1: ['', [Validators.required]],
    GblV2: ['', [Validators.required]],
    MtsV1: ['', [Validators.required]],
    FromDate: ['', [Validators.required]],
    ToDate: ['', [Validators.required]],
    date: null,
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new VinhCuaLoFilter()
  paginationResult = new PaginationResult()
  goodsResult: any[] = []
  loading: boolean = false
  VINH_CUA_LO_RIGHTS = VINH_CUA_LO_RIGHTS
  constructor(
    private _service: VinhCuaLoService,
    private _goodsService: GoodsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách loại hàng hoá',
        path: 'master-data/goods',
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
    this._service.searchVinhCuaLo(this.filter).subscribe({
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
        FromDate: new Date(result[0].setHours(result[0].getHours() + 7)),
        ToDate: new Date(result[1].setHours(result[1].getHours() + 7)),
      })
    }else{
      return
    }
  }

  exportExcel() {
    return this._service
      .exportExcelVinhCuaLo(this.filter)
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
      console.log(formData);

      if (this.edit) {
        this._service.updateVinhCuaLo(formData).subscribe({
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
        this._service.createVinhCuaLo(formData).subscribe({
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
    this.filter = new VinhCuaLoFilter()
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
      GblcsV1: data.gblcsV1,
      GblV2: data.gblV2,
      MtsV1: data.mtsV1,
      FromDate: data.fromDate,
      ToDate: data.toDate,
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
