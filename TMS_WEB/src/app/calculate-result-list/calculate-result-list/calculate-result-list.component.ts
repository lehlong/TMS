import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { CALCULATE_RESULT_LIST_RIGHTS, CALCULATE_RESULT_RIGHT } from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { CalculateResultListFilter } from '../../models/calculate-result-list/calculate-result-list.model'
import { CalculateResultListService } from '../../services/calculate-result-list/calculate-result-list.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { Router } from '@angular/router'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './calculate-result-list.component.html',
  styleUrl: './calculate-result-list.component.scss',
})
export class CalculateResultListComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    name: ['', [Validators.required]],
    status: [true],
    fDate: [''],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new CalculateResultListFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  lst: any[] = []
  CALCULATE_RESULT_LIST_RIGHTS = CALCULATE_RESULT_LIST_RIGHTS

  CALCULATE_RESULT_RIGHT = CALCULATE_RESULT_RIGHT
  constructor(
    private _service: CalculateResultListService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
    private _goodsService: GoodsService,
    private router: Router
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách đợt nhập',
        path: 'master-data/calculate-result-list',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }
  model : any = {
    header: {},
    hS1: [],
    hS2: [],
  }
  goodsResult: any[] = []
  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search();
    this.getAllGoods();
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
    this._service.getall().subscribe({
      next: (data) => {
        this.lst = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some((local: any) => local.code === code)
  }
  submitForm(): void {
    console.log(this.model)
    var m = {
      model: this.model
    }
    this._service.createData(this.model).subscribe({
      next: (data) => {
        console.log(data)
      }
    })
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new CalculateResultListFilter()
    this.search()
  }

  openCreate() {
    this.edit = false
    this.visible = true
    this._service.getObjectCreate().subscribe({
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

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string | number) {
    this._service.deleteCalculateResultList(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  openEdit(data: any) {
    this.router.navigate([`/calculate-result/detail/${data.code}`])
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

  onKeyUpCalculate(row : any){
    row.v2_V1 = row.gblV2 - row.gblcsV1
    row.gny = row.gblcsV1 + row.mtsV1
    row.clgblv = row.gblV2 - row.gny
  }
}
