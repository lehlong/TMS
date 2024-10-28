import { Component, HostListener } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { AccountFilter } from '../../models/system-manager/account.model'
import { TemplateListTablesDataFilter } from '../../models/master-data/template-list-tables-data.model'
import { LIST_AUDIT_TABLE_RIGHTS } from '../../shared/constants'
import {
  NzUploadChangeParam,
  NzUploadFile,
  NzUploadXHRArgs,
} from 'ng-zorro-antd/upload'
import { environment } from '../../../environments/environment'
import { AuditPeriodListTablesService } from '../../services/master-data/audit-period-list-tables.service'
import { OrganizeService } from '../../services/system-manager/organize.service'
import { ListTablesService } from '../../services/business/list-table.service'
import { TemplateListTablesService } from '../../services/master-data/template-list-tables.service'
import { AccountService } from '../../services/system-manager/account.service'
import { GlobalService } from '../../services/global.service'
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http'
import { Subscription } from 'rxjs'
import { tap } from 'rxjs/operators'
import { NzMessageService } from 'ng-zorro-antd/message'
import { v4 as uuidv4 } from 'uuid'
import { ActivatedRoute, Router } from '@angular/router'
@Component({
  selector: 'app-preparing-template-list-table',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './preparing-template-list-table.component.html',
  styleUrl: './preparing-template-list-table.component.scss',
})
export class PreparingTemplateListTableComponent {
  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  listAuditStatus: string = '';
  mgListTablesCode: string = '';
  username = ''
  temDetail: any
  code: number = 0
  templateListTablesCode: string = ''
  templateListTables: any = []
  auditTemplateListTables: any = []
  temData: any
  editId: string | null = null
  isModalVisible = false
  modalTitle = ''
  modalContent = ''
  currentEditDataAudit: any = null
  currentEditDataNote: any = null
  auditPeriodListTables: any = []
  organize: any = []
  listTables: any = []
  account: any[] = []
  accountsfilter = new AccountFilter()
  filter = new TemplateListTablesDataFilter()
  loading: boolean = false
  templateListDataAudit: any[] = []
  LIST_AUDIT_TABLE_RIGHTS = LIST_AUDIT_TABLE_RIGHTS
  sortedData: any[] = []
  isAuditNotesModalVisible = false
  isExplanationModalVisible = false
  auditNotesModalContent: string = ''
  explanationModalContent: string = ''
  isUploadModalVisible = false
  isUploading = false
  fileToUpload: File | null = null
  fileList: NzUploadFile[] = []
  totalAuditValue: number = 0
  totalExplanationValue: number = 0
  referenceId: string = ''
  canViewOrg = false
  canViewAll = false
  canUpdateAudit = false
  canUpdateExplanation = false
  exportAll = false
  exportOrg = false
  isCancelModalVisible = false
  isReviewModalVisible = false
  isApprovalModalVisible = false
  isConfirmModalVisible = false
  cancelModalTitle = ''
  cancelOpinionText = ''
  reviewOpinionText = ''
  confirmOpinionText = ''
  currentData: any
  currentAction = ''
  public baseUrl = environment.baseApiUrl
  constructor(
    private _auditPeriodService: AuditPeriodListTablesService,
    private _orgs: OrganizeService,
    private _ltbs: ListTablesService,
    private _tem: TemplateListTablesService,
    private _acs: AccountService,
    private globalService: GlobalService,
    private msg: NzMessageService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Đợt kiểm toán',
        path: 'master-data/list-audit',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    const UserInfo = this.globalService.getUserInfo()
    this.username = UserInfo?.userName
    this.canViewOrg = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.XEM_THEO_DON_VI,
    )
    this.canViewAll = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.XEM_TAT_CA,
    )
    this.canUpdateAudit = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.EDIT_KIEM_TOAN,
    )
    this.canUpdateExplanation = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.EDIT_GIAI_TRINH,
    )
    this.exportAll = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.EXPORT_ALL,
    )
    this.exportOrg = this.globalService.checkPermissions(
      LIST_AUDIT_TABLE_RIGHTS.EXPORT_ORG,
    )
  }


  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit() {
    this.route.params.subscribe(async (params) => {
      this.code = params['code']
      this.loadInit()
      this.sortData(this.templateListDataAudit)
    })
    const navigation = this.router.getCurrentNavigation();
    if (navigation && navigation.extras && navigation.extras.state) {
      this.listAuditStatus = navigation.extras.state['listAuditStatus'];
    }
    console.log(this.listAuditStatus);

  }
  async loadInit() {
    await this.getAllOrganize()
    await this.getAllAccount()
    await this.getAllAuditPeriodListTables()
    await this.getAllTemplate()
    this.search()
    await this.getAllListTables()
    this.getNameTemplate()
    this.sortData(this.templateListDataAudit)

  }
  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }
  getMatchedAccountAndOrg() {
    const matchedAccount = this.account.find(
      (acc) => acc.userName === this.username,
    )
    if (!matchedAccount) {
      console.error('Không tìm thấy tài khoản phù hợp')
      return null
    }

    const matchedOrg = this.organize.find(
      (org: { id: any }) => org.id === matchedAccount.organizeCode,
    )
    if (!matchedOrg) {
      console.error('Không tìm thấy tổ chức phù hợp')
      return null
    }

    return { matchedAccount, matchedOrg }
  }
  getTemplateList(): any | null {
    return (
      this.templateListTables.find(
        (tem: any) => tem.code === this.templateListTablesCode,
      ) || null
    )
  }
  search() {
    this.isSubmit = false
    const auditListTables = this.auditPeriodListTables.find(
      (adudit: any) => adudit.code === Number(this.code),
    )

    this.templateListTablesCode = auditListTables ? auditListTables.listTablesCode : null
    this.mgListTablesCode = this.templateListTables.find((tem: any) => tem.code === this.templateListTablesCode)?.mgListTablesCode
    const match = this.getMatchedAccountAndOrg()
    if (!match) {
      console.error('Không thể tìm thấy tài khoản hoặc tổ chức phù hợp')
      return
    }
    const { matchedOrg } = match
    const data = {
      auditListTablesCode: this.code,
      orgCode: matchedOrg.id,
    }
    console.log(this.canViewAll, LIST_AUDIT_TABLE_RIGHTS.XEM_TAT_CA)

    if (this.canViewAll) {

      this._auditPeriodService
        .GetTemDataWithMgListTablesAndOrgCode({
          auditListTablesCode: this.code,
        })

        .subscribe({
          next: (data) => {
            console.log('data', data)
            this.templateListDataAudit = data
            this.auditTemplateListTables = this.auditPeriodListTables.find(
              (tem: any) => tem.code === Number(this.code),
            )
            this.sortData(this.templateListDataAudit)
            this.calculateTotals()
          },
          error: (response) => {
            console.log(response)
          },
        })
    } else if (this.canViewOrg) {
      this._auditPeriodService
        .GetTemDataWithMgListTablesAndOrgCode(data)
        .subscribe({
          next: (data) => {
            this.templateListDataAudit = data
            this.auditTemplateListTables = this.auditPeriodListTables.find(
              (tem: any) => tem.code === Number(this.code),
            )
            this.sortData(this.templateListDataAudit)
            this.calculateTotals()
          },
          error: (response) => {
            console.log(response)
          },
        })
    }
  }
  onInputChange(data: any, field: string, value: any): void {
    data[field] = value
  }

  calculateTotals() {
    this.totalAuditValue = this.sortedData.reduce(
      (sum, item) => sum + (Number(item.auditValue) || 0),
      0,
    )
    this.totalExplanationValue = this.sortedData.reduce(
      (sum, item) => sum + (Number(item.explanationValue) || 0),
      0,
    )
  }
  exportExcel() {
    this.loading = true
    const templateList = this.getTemplateList()
    const match = this.getMatchedAccountAndOrg()
    if (!match) {
      console.error('Không thể tìm thấy tài khoản hoặc tổ chức phù hợp')
      return
    }

    const { matchedOrg } = match

    const data = {
      auditListTablesCode: this.code,
      orgCode: matchedOrg.id,
    }
    if (this.exportAll) {
      return this._auditPeriodService
        .exportExcelListTables({ auditListTablesCode: this.code })
        .subscribe((result: Blob) => {
          const blob = new Blob([result], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          })
          const url = window.URL.createObjectURL(blob)
          var anchor = document.createElement('a')
          anchor.download = templateList.name + '.xlsx'
          anchor.href = url
          this.loading = false
          anchor.click()
        })
    } else if (this.exportOrg) {
      return this._auditPeriodService
        .exportExcelListTables(data)
        .subscribe((result: Blob) => {
          const blob = new Blob([result], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          })
          const url = window.URL.createObjectURL(blob)
          var anchor = document.createElement('a')
          anchor.download = templateList.name + '.xlsx'
          anchor.href = url
          this.loading = false
          anchor.click()
        })
    }
    return
  }
  importExcelAndUpdateData(file: File) {
    if (this.canUpdateAudit && this.listAuditStatus === 'Khởi tạo' && this.auditTemplateListTables.status === '1') {
      this._auditPeriodService.uploadExcelSTC(file, this.code).subscribe({
        next: (result) => {
          console.log('Data imported successfully', result)
          this.search() // Refresh data after import
        },
        error: (error) => {
          console.error('Error importing data:', error)
        },
      })
    } else if (this.canUpdateExplanation && this.listAuditStatus === 'Đã phê duyệt' && this.auditTemplateListTables.status === '4') {
      this._auditPeriodService.uploadExcelDV(file, this.code).subscribe({
        next: (result) => {
          console.log('Data imported successfully', result)
          this.search() // Refresh data after import
        },
        error: (error) => {
          console.error('Error importing data:', error)
        },
      })
    }
  }
  saveData(): void {
    const updatedData = this.sortedData.map((item) => ({
      templateDataCode: item.templateDataCode,
      auditListTablesCode: this.code,
      unit: item.unit,
      auditValue: item.auditValue,
      auditExplanation: item.auditExplanation,
      explanationValue: item.explanationValue,
      explanationNote: item.explanationNote,
      isActive: true,
    }))
    console.log('updatedData', updatedData)

    this._auditPeriodService
      .updateAuditTemplateListTables(updatedData)
      .subscribe({
        next: (response) => {
          this.loadInit() // Refresh the data
        },
        error: (error) => {
          console.error('Error updating data:', error)
        },
      })
  }
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    const formData = new FormData()
    this.referenceId = uuidv4()
    formData.append('file', item.file as any)
    formData.append('referenceId', this.referenceId || '')
    let params = new HttpParams()
    if (this.referenceId) {
      params = params.set('referenceId', this.referenceId)
    }
    return this.http
      .post(this.baseUrl + '/ModuleAttachment/Upload', formData, {
        params: params,
        reportProgress: true,
        observe: 'events',
      })
      .pipe(
        tap((event) => {
          if (event.type === HttpEventType.UploadProgress && item.onProgress) {
            item.onProgress(event, item.file)
          }
          if (event.type === HttpEventType.Response && item.onSuccess) {
            item.onSuccess(event.body, item.file, this.fileList)
          }
        }),
      )
      .subscribe({
        error: (err) => {
          if (item.onError) {
            item.onError(err, item.file)
          } else {
            console.error('onError function is not defined')
          }
        },
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
  async getAllOrganize() {
    try {
      this.organize = await this._orgs.getOrg().toPromise()
    } catch (error) {
      console.log(error)
    }
  }

  async getAllAccount() {
    try {
      const response = await this._acs.search(this.accountsfilter).toPromise()
      this.account = response.data
    } catch (error) {
      console.log(error)
    }
  }
  async getAllListTables() {
    try {
      const nodeTree = await this._ltbs.GetLtbTreeWithMgCode(this.mgListTablesCode).toPromise()
      this.listTables = this.flattenTreeData(nodeTree)
      console.log("list: ", this.listTables);

    } catch (error) {
      console.log(error)
    }
  }
  flattenTreeData(treeData: any): any[] {
    let result: any[] = [];

    function flatten(node: any) {
      result.push({
        code: node.code,
        id: node.id,
        name: node.name,
        pId: node.pId,
        title: node.title,
        orderNumber: node.orderNumber,
        mgCode: node.mgCode,
        currencyCode: node.currencyCode,
        isLeaf: node.isLeaf,

        // Add other properties you need
      });

      if (node.children && Array.isArray(node.children)) {
        for (let child of node.children) {
          flatten(child);
        }
      }
    }

    if (Array.isArray(treeData)) {
      for (let node of treeData) {
        flatten(node);
      }
    } else if (treeData) {
      flatten(treeData);
    }

    return result;
  }
  async getAllAuditPeriodListTables() {
    try {
      this.auditPeriodListTables = await this._auditPeriodService
        .getall()
        .toPromise()
    } catch (error) {
      console.log(error)
    }
  }
  async getAllTemplate() {
    try {
      this.templateListTables = await this._tem.getall().toPromise()
    } catch (error) {
      console.log(error)
    }
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
    console.log("sortData", this.sortedData)
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
  @HostListener('document:click', ['$event'])
  handleClickOutside(event: Event) {
    if (
      this.editId &&
      !(event.target as HTMLElement).closest('.editable-cell') &&
      !(event.target as HTMLElement).closest('input')
    ) {
      this.stopEdit()
    }
  }
  showUploadModal(): void {
    this.isUploadModalVisible = true
    this.fileList = []
    this.fileToUpload = null
  }

  handleUploadCancel(): void {
    this.isUploadModalVisible = false
    this.fileList = []
    this.fileToUpload = null
  }

  handleUploadOk(): void {
    if (this.fileToUpload) {
      this.isUploading = true
      this.importExcelAndUpdateData(this.fileToUpload)
      this.isUploadModalVisible = false
    } else {
      console.error('No file selected')
      // Optionally, show an error message to the user
    }
  }
  getNameTemplate() {

    const templateList = this.templateListTables.find(
      (tem: any) => tem.code === this.templateListTablesCode,
    )

    return templateList ? templateList.name : null
  }
  beforeUpload = (file: NzUploadFile): boolean => {
    const isXlsx =
      file.type ===
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    const isXls = file.type === 'application/vnd.ms-excel' // MIME type cho file .xls
    const templateList = this.getTemplateList()
    if (templateList) {
      const expectedFileName = templateList.name + '.xlsx'
      if (file.name !== expectedFileName) {
        this.msg.error(
          `Tên file không đúng. Vui lòng upload file có tên "${expectedFileName}"`,
        )
        return false
      }
    }
    if (!isXlsx && !isXls) {
      this.msg.error('Sai định dạng file. Vui lòng cho file .xlsx hoặc .xls')
      return false
    }

    this.fileToUpload = file as unknown as File
    this.fileList = [file]
    return true
  }

  handleFileChange({ file, fileList }: NzUploadChangeParam): void {
    const status = file.status
    if (status !== 'uploading') {
      console.log(file)
    }
    if (status === 'done') {
      this.fileToUpload = file.originFileObj as File
      this.msg.success(`File ${file.name} upload thành công.`)
    } else if (status === 'error') {
      this.fileToUpload = null
      this.msg.error(`File ${file.name} upload thất bại.`)
    } else if (status === 'removed') {
      this.fileToUpload = null
      this.msg.remove(`Đã xóa file ${file.name}.`)
    }
    this.fileList = fileList
  }
  isEditableRow(data: any): boolean {

    const listTable = this.listTables.find(
      (x: { code: string }) => x.code === data.listTablesCode
    );

    return listTable ? listTable.isLeaf : true;
  }
  startEdit(code: string): void {
    this.editId = code;

  }


  stopEdit(): void {
    this.editId = null
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
        this._auditPeriodService.ChangeStatusCancel(this.currentData.code, this.currentAction, this.cancelOpinionText).subscribe({
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
        this._auditPeriodService.ChangeStatusConfirm(this.currentData.code, this.cancelModalTitle, this.cancelOpinionText).subscribe({
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
    this._auditPeriodService
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
    this._auditPeriodService
      .ChangeStatusApproval(this.currentData.code)
      .subscribe({
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
    this._auditPeriodService.ChangeStatusConfirm(this.currentData.code, action, content).subscribe({
      next: (result) => {
        this.loadInit()
        this.isConfirmModalVisible = false
      },
      error: (error) =>
        console.error('Error changing status to Confirm:', error),
    })
  }
  showReadOnlyMessage(): void {
    this.msg.warning('Dòng này không thể chỉnh sửa.');
  }
  openAuditNotesModal(data: any): void {
    if (this.isEditableRow(data)) {
      this.isAuditNotesModalVisible = true
      this.auditNotesModalContent = data.auditExplanation
      this.currentEditDataAudit = { ...data }
    } else {
      this.showReadOnlyMessage();
    }
  }

  openExplanationModal(data: any): void {
    if (this.isEditableRow(data)) {
      this.isExplanationModalVisible = true
      this.explanationModalContent = data.explanationNote
      this.currentEditDataNote = { ...data }
    } else {
      this.showReadOnlyMessage();
    }
  }

  handleAuditNotesOk(): void {
    if (this.currentEditDataAudit) {
      if (this.canUpdateAudit && this.listAuditStatus === 'Khởi tạo' && this.auditTemplateListTables.status === '1') {
        this.updateDataItem({
          templateDataCode: this.currentEditDataAudit.templateDataCode,
          auditListTablesCode: this.code,
          auditExplanation: this.auditNotesModalContent,
        })
      } else {
        this.auditNotesModalContent =
          this.currentEditDataAudit.auditExplanation || ''
        this.msg.warning('Bạn không có quyền chỉnh sửa giá trị này')
      }
    }
    this.isAuditNotesModalVisible = false
  }

  handleExplanationOk(): void {
    if (this.currentEditDataNote) {
      if (this.canUpdateExplanation && this.listAuditStatus === 'Đã phê duyệt' && this.auditTemplateListTables.status === '4') {
        this.updateDataItem({
          templateDataCode: this.currentEditDataNote.templateDataCode,
          auditListTablesCode: this.code,
          explanationNote: this.explanationModalContent,
        })
      } else {
        this.explanationModalContent =
          this.currentEditDataNote.explanationNote || ''
        this.msg.warning('Bạn không có quyền chỉnh sửa giá trị này')
      }
    }
    this.isExplanationModalVisible = false
  }

  handleAuditNotesCancel(): void {
    this.isAuditNotesModalVisible = false
    this.auditNotesModalContent =
      this.currentEditDataAudit.auditExplanation || ''
  }

  handleExplanationCancel(): void {
    this.isExplanationModalVisible = false
    // Reset the content if you don't want to keep unsaved changes
    this.explanationModalContent =
      this.currentEditDataNote.explanationNote || ''
  }

  updateDataItem(updatedItem: any): void {
    const index = this.sortedData.findIndex(
      (item) => item.templateDataCode === updatedItem.templateDataCode,
    )

    if (index !== -1) {
      // Chỉ cập nhật các trường cụ thể đã thay đổi
      this.sortedData[index] = {
        ...this.sortedData[index],
        auditExplanation:
          updatedItem.auditExplanation !== undefined
            ? updatedItem.auditExplanation
            : this.sortedData[index].auditExplanation,
        explanationNote:
          updatedItem.explanationNote !== undefined
            ? updatedItem.explanationNote
            : this.sortedData[index].explanationNote,
      }
    }
  }
}
