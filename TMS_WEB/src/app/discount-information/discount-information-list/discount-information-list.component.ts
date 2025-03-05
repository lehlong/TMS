import { Component } from '@angular/core'
import { DiscountInformationListService } from '../../services/discount-information/discount-information-list.service'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import {
  COMPETITOR_ANALYSIS,
  DISCOUNT_INFORMATION_LIST_RIGHTS,
} from '../../shared/constants'
import { DiscountInformationListFilter } from '../../models/discount-information-list/discount-information-list.model'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { Router } from '@angular/router'
import { PaginationResult } from '../../models/base.model'
import { GoodsService } from '../../services/master-data/goods.service'
import { CompetitorService } from '../../services/master-data/competitor.service'
import { CalculateResultListService } from '../../services/calculate-result-list/calculate-result-list.service'
import { NzMessageService } from 'ng-zorro-antd/message'

@Component({
  selector: 'app-discount-information-list',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './discount-information-list.component.html',
  styleUrl: './discount-information-list.component.scss',
})
export class DiscountInformationListComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    name: ['', [Validators.required]],
    fDate: [''],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  paginationResult = new PaginationResult()
  filter = new DiscountInformationListFilter()
  visible: boolean = false
  visibleDiscount: boolean = false
  isName: boolean = false
  goodsResult: any[] = []
  competitorResult: any[] = []
  code: any = null
  edit: boolean = false
  model: any = {
    goodss: [
      {
        code: '',
        hs: [],
      },
    ],
    header: {
      code: '',
      name: '',
      fDate: '',
    },
  }
  list: any[] = []
  lstCaculate: any[] = []
  loading: boolean = false
  COMPETITOR_ANALYSIS = COMPETITOR_ANALYSIS

  constructor(
    private fb: NonNullableFormBuilder,
    private _service: DiscountInformationListService,
    private _caculateResultServicer: CalculateResultListService,
    private _goodsService: GoodsService,
    private _competitorService: CompetitorService,
    private message: NzMessageService,
    private globalService: GlobalService,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách Phân tích',
        path: 'discount-information-list',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }

  ngOnInit() {
    this.search()
    this.getAllGoods()
    this.getAllCompetitor()
    this.searchLstCaculate()
  }

  search() {
    this.isSubmit = false
    this._service.searchDiscountInformationList(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  searchLstCaculate() {
    this.isSubmit = false
    this._caculateResultServicer.getall().subscribe({
      next: (data) => {
        this.lstCaculate = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAll() {
    this._service.getAll().subscribe({
      next: (data) => {
        this.list = data
        console.log(this.list)
      },
      error: (resp) => {
        console.log(resp)
      },
    })
  }

  submitForm(): void {
    if (this.model.header.name == '') {
      this.message.error(
        `Vui lòng nhập tên đợt nhập`,
      )
      // return
    }
    if (this.model.header.name != '') {
      this._service.createData(this.model).subscribe({
        next: (data) => {
          console.log(data)
        },
      })
    }
  }

  openCreate() {
    this.edit = false
    this.visible = true
    // this._service.getObjectCreate(this.code).subscribe({
    //   next: (data) => {
    //     this.model = data
    //     console.log(this.model)
    //     this.visible = true
    //   },
    //   error: (err) => {
    //     console.log(err)
    //   },
    // })
  }

  getOjCreByCode(){
    this.visibleDiscount = true
    this._service.getObjectCreate(this.code).subscribe({
      next: (data) => {
        this.model = data
        console.log(this.model)
        this.visible = true
      },
      error: (err) => {
        console.log(err)
      },
    })
  }

  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some((local: any) => local.code === code)
  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new DiscountInformationListFilter()
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }

  openEdit(data: any) {
    this.router.navigate([`/discount-information/detail/${data.code}`])
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  getAllCompetitor() {
    this.isSubmit = false
    this._competitorService.getall().subscribe({
      next: (data) => {
        this.competitorResult = data
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

  checkName(_name: string) {
    _name == '' ? (this.isName = true) : (this.isName = false)
  }
}
