import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { LAIGOPDIEUTIET_RIGHTS, MASTER_DATA_MANAGEMENT } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { LaiGopDieuTietFilter } from '../../models/master-data/lai-gop-dieu-tiet.model'
import { LaiGopDieuTietService } from '../../services/master-data/lai-gop-dieu-tiet.service'
import { MarketService } from '../../services/master-data/market.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './lai-gop-dieu-tiet.component.html',
  styleUrl: './lai-gop-dieu-tiet.component.scss',
})
export class LaiGopDieuTietComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    goodsCode: ['', [Validators.required]],
    marketCode: ['', [Validators.required]],
    price: ['', [Validators.required]],
    fromDate: ['', [Validators.required]],
    toDate: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new LaiGopDieuTietFilter()
  paginationResult = new PaginationResult()
  goodsResult: any[] = []
  marketResult: any[] = []
  marketList: any[] = []
  createDate: string = ''
  errorDate: string = ''
  today = new Date()
  loading: boolean = false
  MASTER_DATA_MANAGEMENT = MASTER_DATA_MANAGEMENT

  constructor(
    private _service: LaiGopDieuTietService,
    private _marketService: MarketService,
    private _goodsService: GoodsService,

    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách lãi gộp điều tiết',
        path: 'master-data/lai-gop-dieu-tiet',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    this.createDate = `${this.today.getDate().toString().padStart(2, '0')}/${(this.today.getMonth() + 1).toString().padStart(2, '0')}/${this.today.getFullYear()}`
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.getAllGoods()
    this.getAllMarket()
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
    this._service.searchLaiGopDieuTiet(this.filter).subscribe({
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
  getAllMarket() {
    this.isSubmit = false
    this._marketService.getall().subscribe({
      next: (data) => {
        this.marketResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  //V1_01-201004-17/10/1024
  autoCreateCode(): void {
    const marketCode = this.validateForm.get('marketCode')?.value
    const goodsCode = this.validateForm.get('goodsCode')?.value
    const newCode = `${marketCode}-${goodsCode}-${this.createDate}`
    this.validateForm.patchValue({ code: newCode })
  }

  exportExcel() {
    return this._service
      .exportExcelLaiGopDieuTiet(this.filter)
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

  checkDate() {
    if (this.validateForm.get('toDate')?.value > this.validateForm.get('fromDate')?.value) {

      return;
    } else {
      this.message.error("Ngày kết thúc phải lớn hơn ngày tạo")
    }
  }

  submitForm(): void {
    this.autoCreateCode()
    this.isSubmit = true
    if (this.validateForm.get('toDate')?.value > this.validateForm.get('fromDate')?.value) {

      if (this.validateForm.valid) {
        const formData = this.validateForm.getRawValue()
        console.log(formData);
        if (this.edit) {
          this._service.updateLaiGopDieuTiet(formData).subscribe({
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
          this._service.createLaiGopDieuTiet(formData).subscribe({
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
    } else {
      this.message.error("Ngày kết thúc phải lớn hơn ngày tạo")
    }
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new LaiGopDieuTietFilter()
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
    this._service.deleteLaiGopDieuTiet(code).subscribe({
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
      price: data.price,
      goodsCode: data.goodsCode,
      marketCode: data.marketCode,
      fromDate: data.fromDate,
      toDate: data.toDate,
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
