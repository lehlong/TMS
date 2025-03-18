import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { GOODS_RIGHTS, GIA_GIAO_TAP_DOAN_RIGHTS, MASTER_DATA_MANAGEMENT } from '../../shared/constants'
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
  // paginationResult = new PaginationResult()
  customerResult: any[] = []
  goodsResult: any[] = []
  data: any = []
  date: Date = new Date()
  loading: boolean = false
  isDateValid: boolean = false
  GOODS_RIGHTS = GOODS_RIGHTS
  MASTER_DATA_MANAGEMENT = MASTER_DATA_MANAGEMENT

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

  model: any = {
    ggtdlHeader: {},
    ggtd: []
  }
  formatDateTime(date: string | null): string {
    if (date) {
      const d = new Date(date);
      // d.setHours(d.getHours() + 5); // Cộng thêm 5 tiếng
      if (d > this.date) {
        this.date = d
        // console.log(this.date);

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
  panels = [
    {
      active: true,
      name: 'This is panel header 1',
      childPanel: [
        {
          active: false,
          name: 'This is panel header 1-1'
        }
      ]
    },
    {
      active: false,
      name: 'This is panel header 2'
    },
    {
      active: false,
      name: 'This is panel header 3'
    }
  ];

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
    this._service.getAll().subscribe({
      next: (data) => {
        this.data = data
        console.log(this.data);
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

  // isCodeExist(code: string): boolean {
  // return this.paginationResult.data?.some((local: any) => local.code === code)
  // }

  checkDate() {
    const date = new Date(this.model.ggtdlHeader.fDate);

    if (this.date > date) {
      this.message.error("Ngày kết thúc phải lớn hơn ngày tạo")
      this.isDateValid = true;
      return;
    }else{
      this.message.error("hợp lệ")

      this.isDateValid = false;
    }

  }


  submitForm(): void {
    this.isSubmit = true

    if (this.model) {

      const formData = this.model
      if (this.edit) {
        this._service.updateGiaGiaoTapDoan(formData).subscribe({
          next: (data) => {
            if (data.oldHeaderGgtd == 'false') {
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
        if (this.date > this.model.ggtdlHeader.fDate) {
          this.message.error("Ngày kết thúc phải lớn hơn ngày đã tạo")
          return;
        }
        this._service.createGiaGiaoTapDoan(formData).subscribe({
          next: (data) => {
              if (data.oldHeaderGgtd == 'false') {
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
    this.filter = new GiaGiaoTapDoanFilter()
    this.search()
  }

  openCreate() {
    this._service.getDataInput().subscribe({
      next: (data) => {
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
