import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { TemplateListTablesDataFilter } from '../../models/master-data/template-list-tables-data.model'
import { PaginationResult } from '../../models/base.model'
import { TEMPLATE_LIST_TABLES_RIGHTS } from '../../shared/constants'
import { TemplateListTablesDataService } from '../../services/master-data/template-list-tables-data.service'
import { OrganizeService } from '../../services/system-manager/organize.service'
import { ListTablesService } from '../../services/business/list-table.service'
import { NonNullableFormBuilder } from '@angular/forms'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute, Router } from '@angular/router'

@Component({
  selector: 'app-template-list-tables-preview',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './template-list-tables-preview.component.html',
  styleUrl: './template-list-tables-preview.component.scss',
})
export class TemplateListTablesPreviewComponent {
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  username = ''
  temDetail: any
  templateCode: string = ''
  temData: any
  //optionsPeriodTime = PeriodTime
  organize: any = []
  listTables: any = []
  filter = new TemplateListTablesDataFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  templateListData: any[] = []
  TEMPLATE_LIST_TABLES_RIGHTS = TEMPLATE_LIST_TABLES_RIGHTS
  sortedData: any[] = []
  constructor(
    private _service: TemplateListTablesDataService,
    private _orgs: OrganizeService,
    private _ltbs: ListTablesService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách template bảng biểu',
        path: 'master-data/template-list-tables',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    const UserInfo = this.globalService.getUserInfo()
    this.username = UserInfo?.userName
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.templateCode = params['code']
      this.getAllOrganize()
      this.getAllListTables()
      this.search()
    })
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
    const code = this.templateCode
    this._service.searchTemplateListTablesData({ KeyWord: code }).subscribe({
      next: (data) => {
        this.paginationResult = data
        this.sortData(this.paginationResult.data)
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  exportExcel() {
    this.loading = true
    const templateCode = this.temDetail
    return this._service
      .exportExcelDataTemplateListTablesData(templateCode)
      .subscribe((result: Blob) => {
        const blob = new Blob([result], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        })
        const url = window.URL.createObjectURL(blob)
        var anchor = document.createElement('a')
        anchor.download = this.temDetail.name + '.xlsx'
        anchor.href = url
        this.loading = false
        anchor.click()
      })
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new TemplateListTablesDataFilter()
    this.search()
  }

  resetForm() {
    this.isSubmit = false
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
  getAllOrganize() {
    this.isSubmit = false
    this._orgs.getOrg().subscribe({
      next: (data) => {
        this.organize = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllListTables() {
    this.isSubmit = false
    this._ltbs.GetAll().subscribe({
      next: (data) => {
        this.listTables = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  sortData(data: any[]) {
    this.sortedData = data.sort((a, b) => {
      // Sort by orgCode first
      if (a.orgCode !== b.orgCode) {
        return a.orgCode.localeCompare(b.orgCode)
      }
      // If orgCode is the same, sort by listTablesCode order
      const orderA = this.getOrderNumber(a.listTablesCode)
      const orderB = this.getOrderNumber(b.listTablesCode)
      return orderA - orderB
    })
  }

  getOrderNumber(listTablesCode: string): number {
    const listTable = this.listTables.find(
      (x: { code: string }) => x.code === listTablesCode,
    )
    return listTable ? listTable.orderNumber : Infinity
  }

  getOrgName(orgCode: string) {
    return this.organize.find((x: { id: string }) => x.id == orgCode)?.name
  }
  getIdListTables(listTablesCode: string) {
    return this.listTables.find(
      (x: { code: string }) => x.code == listTablesCode,
    )?.id
  }
  getNamesListTables(listTablesCode: string) {
    return this.listTables.find(
      (x: { code: string }) => x.code == listTablesCode,
    )?.name
  }
}
