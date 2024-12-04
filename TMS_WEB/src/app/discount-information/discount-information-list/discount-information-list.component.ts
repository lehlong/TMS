import { Component } from '@angular/core';
import { DiscountInformationListService } from '../../services/discount-information/discount-information-list.service';
import { ShareModule } from '../../shared/share-module';
import { GlobalService } from '../../services/global.service';
import { DISCOUNT_INFORMATION_LIST_RIGHTS } from '../../shared/constants'
import { DiscountInformationListFilter } from '../../models/discount-information-list/discount-information-list.model'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PaginationResult } from '../../models/base.model';
import { GoodsService } from '../../services/master-data/goods.service';
import { CompetitorService } from '../../services/master-data/competitor.service';

@Component({
  selector: 'app-discount-information-list',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './discount-information-list.component.html',
  styleUrl: './discount-information-list.component.scss'
})
export class DiscountInformationListComponent{
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
  goodsResult: any[] = []
  competitorResult: any[] = []
  code: any = null
  edit: boolean = false
  model: any = {
    goodss: [{
      code: '',
      hs: []
    }],
    header: {
      name: '',
      fDate: ''
    }
  }
  list: any[] = []
  loading: boolean = false
  DISCOUNT_INFORMATION_LIST_RIGHTS = DISCOUNT_INFORMATION_LIST_RIGHTS

  constructor(
    private fb: NonNullableFormBuilder,
    private _service: DiscountInformationListService,
    private _goodsService: GoodsService,
    private _competitorService: CompetitorService,
    private globalService: GlobalService,
    private router: Router
  ){
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách Phân tích',
        path: 'discount-information-list',
      }
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }

  ngOnInit(){
    this.getAll()
    this.getAllGoods()
    this.getAllCompetitor()
  }

  getAll(){
    this._service.getAll().subscribe({
      next: (data) => {
        this.list = data
        console.log(this.list);
      },
      error: (resp) => {
        console.log(resp);
      }
    })
  }

  submitForm(): void {
    this._service.createData(this.model).subscribe({
      next: (data) => {
        console.log(data)
      }
    })
  }
  openCreate() {
    this.edit = false
    this.visible = true
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
    this.getAll()
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
    this.getAll()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.getAll()
  }

  openEdit(data: any) {
    this.router.navigate([`/discount-information/detail/${data.code}`])
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.getAll()
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

}
