import { Component, Input } from '@angular/core'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { AuditPeriodListTablesFilter } from '../../models/master-data/audit-period-list-tables.model'
import { AccountFilter } from '../../models/system-manager/account.model'
import { PaginationResult } from '../../models/base.model'
import { AuditPeriodFilter } from '../../models/master-data/audit-period.model'
import { LIST_AUDIT_TABLE_RIGHTS } from '../../shared/constants'
import {
  AUDITPERIODLISTTABLES,
  AuditPeriodListTablesState,
} from '../../shared/constants/audit-period-list-tables'
import { AuditPeriodListTablesService } from '../../services/master-data/audit-period-list-tables.service'
import { OrganizeService } from '../../services/system-manager/organize.service'
import { AccountService } from '../../services/system-manager/account.service'
import { AuditPeriodService } from '../../services/master-data/audit-period.service'
import { TemplateListTablesService } from '../../services/master-data/template-list-tables.service'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute, Router } from '@angular/router'
import { ShareModule } from '../../shared/share-module'

import { TemplateListTablesGroupsService } from '../../services/master-data/template-list-tables-groups.service'

@Component({
  selector: 'app-audit-period-list-tables',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './audit-period-list-tables.component.html',
  styleUrl: './audit-period-list-tables.component.scss',
})
export class AuditPeriodListTablesComponent {
  @Input() lstAuditCode: string = ''
  @Input() listAuditStatus: string = '';

  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    listTablesCode: ['', [Validators.required]],
    orgCode: [''],
    auditPeriodCode: ['', [Validators.required]],
    version: ['', [Validators.required]],
    status: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new AuditPeriodListTablesFilter()
  accountsfilter = new AccountFilter()
  paginationResult = new PaginationResult()
  auditPeriodfilter = new AuditPeriodFilter()
  loading: boolean = false
  isVisible = false
  username = ''
  account: any[] = []
  organize: any = []
  templateListTables: any = []
  listOfTables: any[] = []
  selectedTables: any[] = []
  previouslyCheckedTables: any[] = []
  itemsToDelete: any[] = []
  auditPeriod: any[] = []
  history: any[] = []
  isCancelModalVisible = false
  isReviewModalVisible = false
  isApprovalModalVisible = false
  isConfirmModalVisible = false
  canView: boolean = false
  cancelModalTitle = ''
  cancelOpinionText = ''
  reviewOpinionText = ''
  confirmOpinionText = ''
  currentData: any
  currentAction = ''
  templateListTablesGroups: any[] = [];
  filteredListOfTables: any[] = [];
  selectedGroup: string = '';
  LIST_AUDIT_TABLE_RIGHTS = LIST_AUDIT_TABLE_RIGHTS
  AUDITPERIODLISTTABLESSTATE = AuditPeriodListTablesState
  AUDITPERIODLISTTABLES = AUDITPERIODLISTTABLES
  constructor(
    private _service: AuditPeriodListTablesService,
    private _orgs: OrganizeService,
    private _template: TemplateListTablesService,
    private _templateGroups: TemplateListTablesGroupsService,
    private _acs: AccountService,
    private _ps: AuditPeriodService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách đợt kiểm toán',
        path: 'master-data/list-audit',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    const UserInfo = this.globalService.getUserInfo()
    this.username = UserInfo?.userName
    this.canView = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.XEM_CHI_TIET,
    )
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }
  ngOnInit(): void {
    this.loadInit()
    this.getAllAccount()
    this.getAllListTables()
    this.getAllAuditPeriod()
    this.loadTemplateListTablesGroups()
  }
  loadInit() {
    this.search()
    this.getHistory()
    this.getOrg()
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
    this._service
      .searchAuditPeriodListTables({ keyword: this.lstAuditCode })
      .subscribe({
        next: (data) => {
          this.paginationResult = data
          console.log("status", this.listAuditStatus);

        },
        error: (response) => {
          console.log(response)
        },

      })
  }
  getHistory() {
    this.isSubmit = false
    this._service.GetHistoryByListAuditCode(this.lstAuditCode).subscribe({
      next: (data) => {
        this.history = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  loadTemplateListTablesGroups() {
    this._templateGroups.getall().subscribe(
      (data) => {
        this.templateListTablesGroups = data;
      },
      (error) => console.error('Error loading groups:', error)
    );
  }

  onTableCheck(checkedTable: any) {
    // Uncheck all other tables
    this.listOfTables.forEach(table => {
      if (table.code !== checkedTable.code) {
        table.isChecked = false;
      }
    });
  }
  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new AuditPeriodListTablesFilter()
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
    this._service.deleteAuditPeriodListTables(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  openOpinionModal(data: any, action: string) {
    this.currentData = data
    this.currentAction = action

    if (action === 'review_parent') {
      this.isReviewModalVisible = true
    } else if (action === 'review_stc') {
      this.isReviewModalVisible = true
    } else if (action === 'confirm') {
      this.isConfirmModalVisible = true
    } else if (action === 'approval') {
      this.isApprovalModalVisible = true
    } else if (this.listAuditStatus === 'Đã phê duyệt') {
      this.isCancelModalVisible = true
      switch (action) {
        case 'reject_stc':
          this.cancelModalTitle = 'Yêu cầu chỉnh sửa'
          break

        case 'cancel-review_dv':
          this.cancelModalTitle = 'Hủy trình duyệt'
          break

        case 'cancel-approval':
          this.cancelModalTitle = 'Hủy phê duyệt'
          break
      }
    } else {
      this.isCancelModalVisible = true
      switch (action) {
        case 'reject_parent':
          this.cancelModalTitle = 'Yêu cầu chỉnh sửa'
          break

        case 'cancel_review_stc':
          this.cancelModalTitle = 'Hủy trình duyệt'
          break
      }
    }
  }
  handleCancelModal() {
    this.isCancelModalVisible = false
    this.cancelOpinionText = ''
  }

  handleCancelOk() {
    switch (this.currentAction) {
      case 'reject_parent':
      case 'cancel_review_stc':
        this._service.ChangeStatusCancel(this.currentData.code, this.currentAction, this.cancelOpinionText).subscribe({
          next: (result) => {
            console.log(`Status changed to ${this.currentAction}:`, result);
            this.loadInit();
            this.isCancelModalVisible = false;
            this.cancelOpinionText = '';
          },
          error: (error) => console.error(`Error changing status to ${this.currentAction}:`, error),
        });
        break;
      case 'reject_stc':
      case 'cancel-review_dv':
      case 'cancel-approval':
        this._service.ChangeStatusConfirm(this.currentData.code, this.cancelModalTitle, this.cancelOpinionText).subscribe({
          next: (result) => {
            console.log(`Status changed to ${this.currentAction}:`, result);
            this.loadInit();
            this.isCancelModalVisible = false;
            this.cancelOpinionText = '';
          },
          error: (error) => console.error(`Error changing status to ${this.currentAction}:`, error),
        });
        break;
      default:
        console.error('Unknown action:', this.currentAction);
    }
  }
  handleReviewCancel() {
    this.isReviewModalVisible = false
    this.reviewOpinionText = ''
  }

  handleReviewOk() {
    this._service
      .ChangeStatusReview(this.currentData.code, this.reviewOpinionText)
      .subscribe({
        next: (result) => {
          console.log('Status changed to Review:', result)
          this.loadInit()
          this.isReviewModalVisible = false
          this.reviewOpinionText = ''
        },
        error: (error) =>
          console.error('Error changing status to Review:', error),
      })
  }
  handleApprovalCancel() {
    this.isReviewModalVisible = false
  }

  handleApprovalOk() {
    this._service.ChangeStatusApproval(this.currentData.code).subscribe({
      next: (result) => {
        this.loadInit()
        this.isApprovalModalVisible = false
      },
      error: (error) =>
        console.error('Error changing status to Approval:', error),
    })
  }
  handleConfirmCancel() {
    this.isConfirmModalVisible = false
  }

  handleConfirmlOk() {
    const action = 'Xác nhận'
    const content = ''
    this._service.ChangeStatusConfirm(this.currentData.code, action, content).subscribe({
      next: (result) => {
        this.loadInit()
        this.isConfirmModalVisible = false
      },
      error: (error) =>
        console.error('Error changing status to Confirm:', error),
    })
  }

  openEdit(data: { code: string; name: string; isActive: boolean }) {
    this.validateForm.setValue({
      code: data.code,
      name: data.name,
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
    this.loadInit()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.loadInit()
  }
  showModal(): void {
    this.isVisible = true
  }
  onGroupChange(groupCode: string) {
    this.selectedGroup = groupCode;
    console.log("GroupCode:", groupCode);

    this.loadTables(groupCode);
  }
  handleOk(): void {
    const selectedTable = this.listOfTables.find(table => table.isChecked);

    // Lọc các bảng đã bỏ chọn
    const deselectedTables = this.listOfTables.filter(
      (table) =>
        !table.isChecked &&
        this.previouslyCheckedTables.includes(table.code) &&
        table.auditPeriodCode !== null,
    )

    // Thêm mới các bảng được chọn
    if (selectedTable) {
      const params = {
        isActive: true,
        listTablesCode: selectedTable.code,
        auditPeriodCode: this.lstAuditCode,
        version: 1,
        status: '1',
      };
      this._service.createAuditPeriodListTables(params).subscribe(
        (res) => {
          console.log('Table created successfully:', res);
          this.loadInit();
        },
        (error) => {
          console.error('Error creating table:', error);
        }
      );
    }

    // Xóa các bảng đã bỏ chọn
    deselectedTables.forEach((table) => {
      if (table.auditPeriodCode) {
        this._service
          .deleteAuditPeriodListTables(table.auditPeriodCode)
          .subscribe(
            (res) => {
              console.log('Table deleted successfully:', res)
              this.search()
            },
            (error) => {
              console.error('Error deleting table:', error)
            },
          )
      }
    })

    // Cập nhật danh sách các bảng đã chọn trước đó
    this.previouslyCheckedTables = this.listOfTables
      .filter((table) => table.isChecked)
      .map((table) => table.code)

    // Gọi hàm search để cập nhật dữ liệu
    this.search()

    this.isVisible = false // Đóng modal
  }
  loadTables(groupCode?: string): void {
    const params = {
      auditPeriodCode: this.lstAuditCode,
      templateGroupsCode: groupCode,
    }
    console.log("Params", params);

    this._service
      .GetTemplateListTablesByAuditPeriodCode(params)
      .subscribe((response) => {
        this.listOfTables = response // Load data into the table
        console.log("listOfTables:", this.listOfTables)

        this.previouslyCheckedTables = this.listOfTables
          .filter((table) => table.isChecked)
          .map((table) => table.code)
      })
  }

  preparing(data: { code: number }) {
    if (this.canView) {
      this._service
        .GetTemDataWithMgListTablesAndOrgCode({
          auditListTablesCode: data.code,
        })
        .subscribe({
          next: (result) => {
            this.router.navigate(
              [
                '/master-data/list-audit-edit/preparing-template-list-table',
                data.code,
              ],
              { state: { templateListDataAudit: result, listAuditStatus: this.listAuditStatus } },
            )
          },
          error: (error) => console.error(error),
        })
    }
  }
  handleCancel(): void {
    this.selectedTables.forEach((unit) => {
      if (unit.isChecked) {
        unit.isChecked = false
      }
    })
    this.isVisible = false
  }
  onUnitCheck(unit: any) {
    const index = this.listOfTables.findIndex((u) => u.code === unit.code)
    if (index !== -1) {
      const previousState = this.listOfTables[index].isChecked
      this.listOfTables[index].isChecked = unit.isChecked

      if (previousState && !unit.isChecked && unit.auditPeriodCode) {
        this.itemsToDelete.push(unit.auditPeriodCode)
      } else if (!previousState && unit.isChecked) {
        const deleteIndex = this.itemsToDelete.indexOf(unit.auditPeriodCode)
        if (deleteIndex > -1) {
          this.itemsToDelete.splice(deleteIndex, 1)
        }
      }
    }
  }
  getAllAuditPeriod() {
    this._ps.searchAuditPeriod(this.auditPeriodfilter).subscribe({
      next: ({ data }) => {
        this.auditPeriod = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getMgListTablesCode(code: number | string) {
    if (!this.paginationResult.data) {
      return null
    }
    return this.paginationResult.data.find(
      (x: { code: string }) => x.code == code,
    )?.listTablesCode
  }
  getNameAuditPeriod(auditPeriod: string) {
    if (!this.auditPeriod) {
      return null
    }
    return this.auditPeriod.find((x: { code: string }) => x.code == auditPeriod)
      ?.auditPeriod
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
  getAllListTables() {
    this.isSubmit = false
    this._template.getall().subscribe({
      next: (data) => {
        this.templateListTables = data
      },
      error: (response) => {
        console.log(response)
      },
    })
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
  getTemplateListTablesName(listTablesCode: string) {
    return this.templateListTables.find(
      (x: { code: string }) => x.code == listTablesCode,
    )?.name
  }

  getOrg() {
    this._orgs.getOrg().subscribe((res) => {
      this.organize = res
    })
  }
  getStatusName(status: string) {
    return this.AUDITPERIODLISTTABLESSTATE.find(
      (x: { value: string }) => x.value == status,
    )?.name
  }
}
