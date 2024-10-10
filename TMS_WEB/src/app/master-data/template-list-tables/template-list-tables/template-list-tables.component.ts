import { Component, ViewChild } from '@angular/core'
import { AccountFilter } from '../../../models/system-manager/account.model'
import { TemplateListTablesFilter } from '../../../models/master-data/template-list-tables.model'
import { PaginationResult } from '../../../models/base.model'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { TEMPLATE_LIST_TABLES_RIGHTS } from '../../../shared/constants'
import { TemplateListTablesService } from '../../../services/master-data/template-list-tables.service'
import { AccountService } from '../../../services/system-manager/account.service'
import { OrganizeService } from '../../../services/system-manager/organize.service'
import { DropdownService } from '../../../services/dropdown/dropdown.service'
import { GlobalService } from '../../../services/global.service'
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree'
import { ShareModule } from '../../../shared/share-module'
import { TemplateListTablesEditComponent } from '../template-list-tables-edit/template-list-tables-edit.component'
import { NzMessageService } from 'ng-zorro-antd/message'
import { ActivatedRoute, Router } from '@angular/router'

@Component({
  selector: 'app-template-list-tables',
  standalone: true,
  imports: [ShareModule, TemplateListTablesEditComponent],
  templateUrl: './template-list-tables.component.html',
  styleUrl: './template-list-tables.component.scss',
})
export class TemplateListTablesComponent {
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  username = ''
  tabIndex: number = 0
  temDetail: any
  nodeCurrent!: any
  //optionsPeriodTime = PeriodTime
  accountsfilter = new AccountFilter()
  account: any[] = []
  organize: any = []
  filter = new TemplateListTablesFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  mgListTablesList: any[] = []
  periodTimeList: any[] = []
  groupCode: any
  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    name: ['', [Validators.required]],
    note: [''],
    orgCode: [''],
    timeYear: ['', [Validators.required]],
    mgListTablesCode: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
    groupCode: [''],
  })
  TEMPLATE_LIST_TABLES_RIGHTS = TEMPLATE_LIST_TABLES_RIGHTS
  @ViewChild(TemplateListTablesEditComponent) templateListTablesEditComponent!: TemplateListTablesEditComponent;

  constructor(
    private _service: TemplateListTablesService,
    private _acs: AccountService,
    private _orgs: OrganizeService,
    private dropdownService: DropdownService,
    private message: NzMessageService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách template ý kiến',
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
      this.groupCode = params['code']
      this.search()
      this.getAllAccount()
      this.getOrg()
      this.getAllMgListTables()
      this.getAllPeriodTime()
      console.log("groupCode", this.groupCode);
    })
  }

  onDragStart(event: any): void {
    // Handle drag start event
  }

  nzEvent(event: NzFormatEmitEvent): void { }
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
    this._service.searchTemplateListTables({ groupCode: this.groupCode }).subscribe({
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
      .exportExcelTemplateListTables(this.filter)
      .subscribe((result: Blob) => {
        const blob = new Blob([result], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        })
        const url = window.URL.createObjectURL(blob)
        var anchor = document.createElement('a')
        anchor.download = 'danh-sach-template-bang-bieu.xlsx'
        anchor.href = url
        anchor.click()
      })
  }
  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some(
      (template: any) => template.code === code,
    )
  }

  submitForm(): void {
    this.isSubmit = true
    if (this.validateForm.valid) {
      this.validateForm.patchValue({
        orgCode: this.username,
        groupCode: this.groupCode,
      })
      const existingTemplate = this.paginationResult.data.find(
        (template: any) =>
          template.mgListTablesCode ===
          this.validateForm.value.mgListTablesCode,
      )


      if (this.edit) {
        this._service
          .updateTemplateListTables(this.validateForm.getRawValue())
          .subscribe({
            next: (data) => {
              this.search()
            },
            error: (response) => {
              console.log(response)
            },
          })
      } else {
        if (existingTemplate) {
          this.message.error(
            'Mỗi template chỉ được phép có một Danh mục bảng biểu duy nhất.',
          )
          return // Ngừng thực hiện nếu đã tồn tại
        }
        const formData = this.validateForm.getRawValue()
        if (this.isCodeExist(formData.code)) {
          this.message.error(
            `Mã Template ${formData.code} đã tồn tại, vui lòng nhập lại`,
          )
          return
        }
        this._service
          .createTemplateListTables(this.validateForm.getRawValue())
          .subscribe({
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
  updateTemplateData() {
    // Gọi phương thức updateTemplateData của TemplateListTablesEditComponent
    if (this.templateListTablesEditComponent) {
      this.templateListTablesEditComponent.updateTemplateData();
    }
  }

  preview() {
    // Gọi phương thức preview của TemplateListTablesEditComponent
    if (this.templateListTablesEditComponent) {
      this.templateListTablesEditComponent.preview();
    }
  }
  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new TemplateListTablesFilter()
    this.search()
  }
  getFullName(createBy: string) {
    return this.account.find(
      (x: { userName: string }) => x.userName == createBy,
    )?.fullName
  }

  getOrgName(createBy: string) {
    const matchedAccount = this.account.find((acc) => acc.userName === createBy)
    if (matchedAccount) {
      const matchedOrg = this.organize.find(
        (org: { id: any }) => org.id === matchedAccount.organizeCode,
      )
      if (matchedOrg) {
        return matchedOrg.name
      }
    }
    return 'N/A'
  }
  changeIsActiveStatus(code: string) {
    this._service.ChangeIsActiveStatus(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getOrg() {
    this._orgs.getOrg().subscribe((res) => {
      this.organize = res
    })
  }
  getAllAccount() {
    this._acs.search(this.accountsfilter).subscribe({
      next: ({ data }) => {
        this.account = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  openCreate() {
    this.edit = false
    this.visible = true
    console.log('Visibility:', this.visible)
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string | number) {
    this._service.deleteTemplateListTables(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAllMgListTables() {
    this.dropdownService
      .getAllMgListTables({
        IsCustomer: true,
        SortColumn: 'name',
        IsDescending: true,
      })
      .subscribe({
        next: (data) => {
          this.mgListTablesList = data
        },

        error: (response) => {
          console.log(response)
        },
      })
  }
  getAllPeriodTime() {
    this.dropdownService
      .getAllPeriodTime({
        IsCustomer: true,
        SortColumn: 'name',
        IsDescending: true,
      })
      .subscribe({
        next: (data) => {
          this.periodTimeList = data
        },

        error: (response) => {
          console.log(response)
        },
      })
  }
  onDataUpdated(templateCode: string) {
    this._service.GetTemWithTreee(templateCode).subscribe((res) => {
      this.nodeCurrent = res;
    });
  }
  openEdit(data: {
    code: string
    name: string
    timeYear: string
    note: string
    orgCode: string
    mgListTablesCode: string
    isActive: boolean
    groupCode: string
  }) {
    this._service.GetTemWithTreee(data.code).subscribe((res) => {
      this.nodeCurrent = res
      this.validateForm.patchValue({
        code: data.code,
        name: data.name,
        timeYear: data.timeYear,
        note: data.note,
        orgCode: data.orgCode,
        mgListTablesCode: data.mgListTablesCode,
        isActive: data.isActive,
        groupCode: data.groupCode,
      })
      console.log(this.validateForm.getRawValue())

      setTimeout(() => {
        this.edit = true
        this.visible = true
        console.log('Visibility:', this.visible)
      }, 200)
    })
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

  onSelectedTab(event: any) {
    this.tabIndex = event
    console.log("tabindex", this.tabIndex);

  }
  handleChildEvent(data: any) {
    this.temDetail = data
  }
}
