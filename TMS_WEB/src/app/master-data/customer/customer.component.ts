import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { LOCAL_RIGHTS, CUSTOMER_RIGHTS } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { CustomerFilter } from '../../models/master-data/customer.model'
import { CustomerService } from '../../services/master-data/customer.service'
import { MarketService } from '../../services/master-data/market.service'
import { SalesMethodService } from '../../services/master-data/sales-method.service'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss',
})
export class CustomerComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    name: ['', [Validators.required]],
    phone: [''],
    email: [''],//, Validators.email
    address: [''],
    gap: ['', [Validators.required]],
    buyInfo: [''],
    bankLoanInterest: [''],

    salesMethodCode: [''],
    localCode: ['', [Validators.required]],
    marketCode: ['', [Validators.required]],
    isActive: [true, [Validators.required]],

  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new CustomerFilter()
  paginationResult = new PaginationResult()
  localResult: any[] = []
  marketResult: any[] = []
  marketList: any[] = []
  salesMethodResult: any[] = []

  loading: boolean = false
  CUSTOMER_RIGHTS = CUSTOMER_RIGHTS

  constructor(
    private _service: CustomerService,
    private _marketService: MarketService,
    private _localService: LocalService,
    private _salesMethodService: SalesMethodService,

    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách khách hàng',
        path: 'master-data/customer',
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
    this.getAllLocal()
    this.getAllSalesMethod()
    this.search()
    this.getAllMarket()
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
    this._service.searchCustomer(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllLocal(){
    this.isSubmit = false
    this._localService.getall().subscribe({
      next: (data) => {
        this.localResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllMarket(){
    this.isSubmit = false
    this._marketService.getall().subscribe({
      next: (data) => {
        this.marketList= data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  searchMarket() {
    this.isSubmit = false
    console.log(this.validateForm.get('localCode')?.value as string);

    this.marketResult = this.marketList.filter(market => market.localCode === this.validateForm.get('localCode')?.value)
  }

  getAllSalesMethod(){
    this.isSubmit = false
    this._salesMethodService.getall().subscribe({
      next: (data) => {
        this.salesMethodResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  exportExcel() {
    return this._service
      .exportExcelCustomer(this.filter)
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
        this._service.updateCustomer(formData).subscribe({
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
        this._service.createCustomer(formData).subscribe({
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
    this.filter = new CustomerFilter()
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
    this._service.deleteCustomer(code).subscribe({
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
      phone: data.phone,
      email: data.email,
      address: data.address,
      gap: data.gap,
      buyInfo: data.buyInfo,
      bankLoanInterest: data.bankLoanInterest,
      salesMethodCode: data.salesMethodCode,
      localCode: data.localCode,
      marketCode: data.marketCode,
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
