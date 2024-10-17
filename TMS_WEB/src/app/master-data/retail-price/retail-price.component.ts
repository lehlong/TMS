import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { LOCAL_RIGHTS, TYPE_OF_GOODS, RETAIL_PRICE_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { RetailPriceFilter } from '../../models/master-data/retail-price.model'
import { RetailPriceService } from '../../services/master-data/retail-price.service'
import { TypeOfGoodsService } from '../../services/master-data/type-of-goods.service'
import { LocalService } from '../../services/master-data/local.service'

@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './retail-price.component.html',
  styleUrl: './retail-price.component.scss',
})
export class RetailPriceComponent {
  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    typeOfGoodsCode: ['', [Validators.required]],
    localCode: ['', [Validators.required]],
    createDate: new Date(),
    toDate: ['', [Validators.required]],
    oldPrice: ['', [Validators.required]],
    newPrice: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new RetailPriceFilter()
  paginationResult = new PaginationResult()
  localResult : any[] = []
  typeOfGoodsResult : any[] = []
  loading: boolean = false
  TYPE_OF_GOODS = TYPE_OF_GOODS
  RETAIL_PRICE_RIGHTS = RETAIL_PRICE_RIGHTS

  constructor(
    private _service: RetailPriceService,
    private _serviceLocal: LocalService,
    private _serviceTypeOfGoods: TypeOfGoodsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách giá bán lẻ',
        path: 'master-data/retail-price',
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
    this.getAllLocal()
    this.getAllTypeOfGoods()
  }

  onSortChange(code: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: code,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  getAllLocal(){
    this._serviceLocal.getall().subscribe({
      next: (data) => {

        this.localResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }

  getAllTypeOfGoods(){
    this._serviceTypeOfGoods.getall().subscribe({
      next: (data) => {
        console.log(data);

        this.typeOfGoodsResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }

  search() {
    this.isSubmit = false
    this._service.searchRetailPrice(this.filter).subscribe({
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
      .exportExcelRetailPrice(this.filter)
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
        this._service.updateRetailPrice(formData).subscribe({
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
        this._service.createRetailPrice(formData).subscribe({
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
    this.filter = new RetailPriceFilter()
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
    this._service.deleteRetailPrice(code).subscribe({
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
      typeOfGoodsCode: data.typeOfGoodsCode,
      localCode: data.localCode,
      startDate: data.createDate,
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
