import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { LOCAL_RIGHTS, GOODS_RIGHTS, RETAIL_PRICE_RIGHTS, MASTER_DATA_MANAGEMENT } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { RetailPriceFilter } from '../../models/master-data/retail-price.model'
import { RetailPriceService } from '../../services/master-data/retail-price.service'
import { GoodsService } from '../../services/master-data/goods.service'
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
    goodsCode: ['', [Validators.required]],
    localCode: ['', [Validators.required]],
    fromDate: ['', [Validators.required]],
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
  localResult: any[] = []
  data: any = []
  isDateValid: boolean = false
  date: Date = new Date()
  goodsResult: any[] = []
  loading: boolean = false
  GOODS_RIGHTS = GOODS_RIGHTS
  MASTER_DATA_MANAGEMENT = MASTER_DATA_MANAGEMENT
  model: any = {
    gbllHeader: {},
    gbl: []
  }

  constructor(
    private _service: RetailPriceService,
    private _serviceLocal: LocalService,
    private _serviceGoods: GoodsService,
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

  getAllLocal() {
    this._serviceLocal.getall().subscribe({
      next: (data) => {
        this.localResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }

  getAllGoods() {
    this._serviceGoods.getall().subscribe({
      next: (data) => {
        this.goodsResult = data
      },
      error: (resp) => {
        console.log(resp)
      }
    })
  }
  formatDateTime(date: string | null): string {
    if (date) {
      const d = new Date(date);
      // d.setHours(d.getHours() + 5); // Cộng thêm 5 tiếng
      if (d > this.date) {
        this.date = d
        console.log(this.date);

      }
      return d.toLocaleString('vi-VN', {
        hour: '2-digit',
        minute: '2-digit',
        day: 'numeric',
        month: 'numeric',
        year: 'numeric'
      });
    }
    return 'Không có ngày giờ';
  }

  search() {
    this.isSubmit = false
    this._service.getall().subscribe({
      next: (data) => {
        this.data = data
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

  checkDate(data: Date) {
    this.date = new Date(this.model.gbllHeader.fDate)
    const tDate = new Date(data)

    if (this.date < tDate) {
      this.isDateValid = false
      return;
    } else {
      this.isDateValid = true
      this.message.error("Ngày kết thúc phải lớn hơn ngày tạo")
    }
  }

  submitForm(): void {
    this.isSubmit = true

    if (this.model) {

      const formData = this.model
      if (this.edit) {
        this._service.updateRetailPrice(formData).subscribe({
          next: (data) => {
            if (data.oldHeaderGbl == 'false') {
              this.message.error(
                `Ngày tạo không được nhỏ hơn ngày đã tạo`,
              )
            } else {
              this.search()
            }
          },
          error: (response) => {
            this.message.error(
              `Ngày tạo không được nhỏ hơn ngày tạo`,
            )
            console.log(response)
          },
        })
      } else {
        if (this.date > this.model.oldHeaderGbl.fDate) {
          this.message.error("Ngày kết thúc phải lớn hơn ngày đã tạo")
          return;
        }
        this._service.createRetailPrice(formData).subscribe({
          next: (data) => {
              if (data.oldHeaderGbl == 'false') {
                this.message.error(
                  `Ngày tạo không được nhỏ hơn ngày đã tạo`,
                )
              } else {
                this.search()
              }
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
    console.log(this.data);
    this._service.getDataInput().subscribe({
      next: (data) => {
        this.date = data.gbllHeader.fDate
        console.log(this.model);
        this.model = data
        console.log(this.model);
        this.edit = false
        this.visible = true
      },
      error: (response) => {
        console.log(response)
      },
    })
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

  openEdit(data: any) {
    console.log(this.date);

    this.model = data
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
