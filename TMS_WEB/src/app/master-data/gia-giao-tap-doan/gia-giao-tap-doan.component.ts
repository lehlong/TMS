import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { GOODS_RIGHTS, GIA_GIAO_TAP_DOAN_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { GiaGiaoTapDoanFilter } from '../../models/master-data/gia-giao-tap-doan.model'
import { GiaGiaoTapDoanService } from '../../services/master-data/gia-giao-tap-doan.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { LocalService } from '../../services/master-data/local.service'
import { CustomerService } from '../../services/master-data/customer.service'

@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './gia-giao-tap-doan.component.html',
  styleUrl: './gia-giao-tap-doan.component.scss',
})
export class GiaGiaoTapDoanComponent {
  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    goodsCode: ['', [Validators.required]],
    customerCode: ['', [Validators.required]],
    fromDate: ['', [Validators.required]],
    toDate: ['', [Validators.required]],
    oldPrice: ['', [Validators.required]],
    newPrice: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new GiaGiaoTapDoanFilter()
  paginationResult = new PaginationResult()
  customerResult: any[] = []
  goodsResult: any[] = []
  loading: boolean = false
  GOODS_RIGHTS = GOODS_RIGHTS
  GIA_GIAO_TAP_DOAN_RIGHTS = GIA_GIAO_TAP_DOAN_RIGHTS

  constructor(
    private _service: GiaGiaoTapDoanService,
    private _goodsService: GoodsService,
    private _customerService: CustomerService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách giá giao tập đoàn',
        path: 'master-data/gia-giao-tap-doan',
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
    this.getAllCustomer()
    this.getAllGoods()
  }

  onSortChange(code: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: code,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  getAllCustomer() {
    this._customerService.getall().subscribe({
      next: (data) => {
        this.customerResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }

  getAllGoods() {
    this._goodsService.getall().subscribe({
      next: (data) => {
        this.goodsResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }

  search() {
    this.isSubmit = false
    this._service.searchGiaGiaoTapDoan(this.filter).subscribe({
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
      .exportExcelGiaGiaoTapDoan(this.filter)
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
    this.isSubmit = true
    if (this.validateForm.get('toDate')?.value > this.validateForm.get('fromDate')?.value) {

      if (this.validateForm.valid) {
        const formData = this.validateForm.getRawValue()
        if (this.edit) {
          this._service.updateGiaGiaoTapDoan(formData).subscribe({
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
          this._service.createGiaGiaoTapDoan(formData).subscribe({
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
    this.filter = new GiaGiaoTapDoanFilter()
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
    this._service.deleteGiaGiaoTapDoan(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  // openEdit(data: any){
  //   console.log(data);

  //   this.edit = true
  //   this.visible = true
  // }

  openEdit(data: any) {
    console.log(data);

    this.validateForm.patchValue({
      code: data.code,
      goodsCode: data.goodsCode,
      customerCode: data.customerCode,
      formDate: data.fromDate,
      toDate: data.toDate,
      oldPrice: data.oldPrice,
      newPrice: data.newPrice,
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
