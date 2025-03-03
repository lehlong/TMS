import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { MapPointCustomerGoodsFilter } from '../../models/master-data/map-point-customer-goods.model'
import { GlobalService } from '../../services/global.service'
import { MapPointCustomerGoodsService } from '../../services/master-data/map-point-customer-goods.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { MAP_POINT_CUSTOMER_GOODS_RIGHTS, MASTER_DATA_MANAGEMENT } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { DeliveryPointService } from '../../services/master-data/delivery-point.service'
import { CustomerService } from '../../services/master-data/customer.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { CustomerTypeService } from '../../services/master-data/customer-type.service'
@Component({
  selector: 'app-map-point-customer-goods',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './map-point-customer-goods.component.html',
  styleUrl: './map-point-customer-goods.component.scss',
})
export class MapPointCustomerGoodsComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    deliveryPointCode: ['', [Validators.required]],
    customerCode: ['', [Validators.required]],
    goodsCode: ['', [Validators.required]],
    cuocVcBq: [''],
    type: [''],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new MapPointCustomerGoodsFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  MASTER_DATA_MANAGEMENT = MASTER_DATA_MANAGEMENT
  lstDeliveryPoint: any[] = []
  lstCustomer: any[] = []
  lstGoods: any[] = []
  lstCustomerType: any[] = []

  constructor(
    private _service: MapPointCustomerGoodsService,
    private _deliveryPointService: DeliveryPointService,
    private _customerService: CustomerService,
    private _customerTypeService: CustomerTypeService,
    private _goodsService: GoodsService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách người nhận hàng',
        path: 'master-data/local',
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
    this.getAllDeliveryPoint()
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

  getAllDeliveryPoint() {
    this.isSubmit = false
    this._deliveryPointService.getall().subscribe({
      next: (data) => {
        this.lstDeliveryPoint = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllCustomer() {
    this.isSubmit = false
    this._customerService.getall().subscribe({
      next: (data) => {
        this.lstCustomer = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllCustomerType() {
    this.isSubmit = false
    this._customerTypeService.getall().subscribe({
      next: (data) => {
        this.lstCustomerType = data
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
        this.lstGoods = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  search() {
    this.isSubmit = false
    this._service.searchMapPointCustomerGoods(this.filter).subscribe({
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
      .exportExcelMapPointCustomerGoods(this.filter)
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
        this._service.updateMapPointCustomerGoods(formData).subscribe({
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
        this._service.createMapPointCustomerGoods(formData).subscribe({
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
    this.filter = new MapPointCustomerGoodsFilter()
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
    this._service.deleteMapPointCustomerGoods(code).subscribe({
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
      deliveryPointCode: data.deliveryPointCode,
      customerCode: data.customerCode,
      goodsCode: data.goodsCode,
      cuocVcBq: data.cuocVcBq,
      type: data.type,
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
