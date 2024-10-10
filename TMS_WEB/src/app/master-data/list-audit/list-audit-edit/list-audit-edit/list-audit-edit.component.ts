import { Component, OnInit, TemplateRef } from '@angular/core'
import { ShareModule } from '../../../../shared/share-module'
import { ListAuditService } from '../../../../services/master-data/list-audit.service'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { GlobalService } from '../../../../services/global.service'
import { ActivatedRoute, Router } from '@angular/router'
import { NzMessageService } from 'ng-zorro-antd/message'
import { MgOpinionListService } from '../../../../services/master-data/mg-opinion-list.service'
import { DropdownService } from '../../../../services/dropdown/dropdown.service'
import { AuditStatus, ListAuditFilter, } from '../../../../models/master-data/list-audit.model'
import { ListAuditInterface } from '../../../../models/master-data/list-audit.model'
import { LIST_AUDIT_OPINION_RIGHTS } from '../../../../shared/constants/access-right.constants'
import { LIST_AUDIT_RIGHTS } from '../../../../shared/constants/access-right.constants'
import { OrganizeService } from '../../../../services/system-manager/organize.service'
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree'
import { OpinionListService } from '../../../../services/business/opinion-list.service'
import { environment } from '../../../../../environments/environment'
import { NzUploadChangeParam, NzUploadFile, NzUploadXHRArgs, } from 'ng-zorro-antd/upload'
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http'
import { Subscription } from 'rxjs'
import { tap } from 'rxjs/operators'
import { AuditPeriodListTablesComponent } from '../../../audit-period-list-tables/audit-period-list-tables.component'
import { RightService } from '../../../../services/system-manager/right.service'
@Component({
  selector: 'app-list-audit-edit',
  standalone: true,
  imports: [ShareModule, AuditPeriodListTablesComponent],
  templateUrl: './list-audit-edit.component.html',
  styleUrl: './list-audit-edit.component.scss',
})
export class ListAuditEditComponent implements OnInit {
  visible: boolean = false
  loading: boolean = false
  opinionList: any[] = []
  listTimeYear: any[] = []
  listAudit: any[] = []
  lstAuditCode: string = ''
  auditCode: string = ''
  AuditStatus = AuditStatus
  audit!: ListAuditInterface
  status: string = ''
  attachments: any[] = []
  attachmentsOpinion: any[] = []
  fileId: string = ''
  createBy: string = ''
  isVisibleAuditModal: boolean = false
  textContent: string = ''
  listAuditHistory: any[] = []
  orgName: string = '';
  organize: any = [];
  users: any = [];
  code = ''
  userName: any = this.globalService.getUserName();
  LIST_AUDIT_OPINION_RIGHTS = LIST_AUDIT_OPINION_RIGHTS
  LIST_AUDIT_RIGHTS = LIST_AUDIT_RIGHTS
  validateForm: FormGroup = this.fb.group({
    code: [{ value: '', disabled: true }, [Validators.required]],
    name: [''],
    timeYear: [''],
    auditPeriod: [''],
    reportDate: [''],
    reportNumber: [''],
    status: [{ value: '', disabled: true }, [Validators.required]],
    startDate: [''],
    endDate: [''],
    note: [''],
    opinionCode: [''],
    fileId: [''],
    textContent: [''],
    approver: [''],
    history: [[]],
    isActive: [true],
  })
  textReport = ''
  userType = ''
  public baseUrl = environment.baseApiUrl
  opinionCodeTree = ''
  nodeOrg: any[] = []
  searchValueOrg = ''
  nodeCurrentOrg!: any
  visibleEditOpinion: boolean = false
  isVisibleModal: boolean = false
  nodeCurrentOpinion: any = {
    name: '',
  }
  nodeOpinion: any[] = []
  searchValueOpinion = ''
  auditPeriod = ''
  timeYear = ''
  approver = ''
  opinionDetail: any = {
    id: '',
    opinionCode: '',
    mgCode: '',
    status: '',
    statusName: '',
    orgInCharge: '',
    createBy: '',
    contentOrg: '',
    contentReport: '',
    action: '',
    textContent: '',
  }
  checkContentOrg = false;
  checkRightEdit = false;
  lstHistoryOpinionDetail: any[] = []
  fileList: NzUploadFile[] = []
  suffixIcon: string | TemplateRef<void> | undefined
  constructor(
    private _service: ListAuditService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private route: ActivatedRoute,
    private router: Router,
    private message: NzMessageService,
    private _mgops: MgOpinionListService,
    private dropDownService: DropdownService,
    private _serviceOrg: OrganizeService,
    private _serviceOpinionList: OpinionListService,
    private msg: NzMessageService,
    private http: HttpClient,
    private _orgs: OrganizeService,
    private _dropDownService: DropdownService,
    private _rightService: RightService
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
  }
  private populateListAuditForm(data: any) {
    this.validateForm.patchValue({
      code: data.code,
      name: data.name,
      timeYear: data.timeYear,
      auditPeriod: data.auditPeriod,
      reportDate: data.reportDate,
      reportNumber: data.reportNumber,
      status: data.status,
      startDate: data.startDate,
      endDate: data.endDate,
      note: data.note,
      opinionCode: data.opinionCode,
      fileId: data.fileId,
      textContent: data.textContent,
      approver: data.approver,
      isActive: data.isActive,
    })
  }
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.lstAuditCode = params['code']
      const navigation = this.router.getCurrentNavigation()
      if (navigation?.extras.state) {
        const state = navigation.extras.state as { ListAuditData: any }
        if (state.ListAuditData) {
          this.populateListAuditForm(state.ListAuditData)
          this.status = state.ListAuditData.status
          this.fileId = state.ListAuditData.fileId
          this.opinionCodeTree = state.ListAuditData.opinionCode
          this.auditCode = state.ListAuditData.code
          this.timeYear = state.ListAuditData.timeYear
          this.auditPeriod = state.ListAuditData.auditPeriod
          this.createBy = state.ListAuditData.createBy
          this.listAuditHistory = state.ListAuditData.history
          this.approver = state.ListAuditData.approver
          this.code = state.ListAuditData.code
        }
      }   
      this.getOpinion()
      this.getTimeYear()
      this.getAudit()
      this.loadAttachments()
      this.getNodeOpinion()
      this.getAllOrganize()
      this.getRightOfUser(this.userName) 
     // this.getNodeOrg()   
    })
  }
  getAllOrganize() {
    this._orgs.getOrg().subscribe({
      next: (data) => {
        this.organize = data
        this.getAllUser()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAllUser() {
    this._dropDownService.GetAllAccount().subscribe({
      next: (data) => {
        this.users = data
        this.getOrgName();
        this.getTypeOfUser(this.userName) 
        this.getNodeOrg() 
      },
      error: (response) => {
        console.log(response)
      }
    })
  }
  getTypeOfUser(userName: string){
    const account = this.users.find((acc: { userName: string }) => acc.userName === userName);
    this.userType =  account.accountType 
  }
  getOrgName() {
    const orgCode = this.users.find((x: { userName: string }) => x.userName === this.createBy).organizeCode;
    const orgName = this.organize.find((x: { id: string }) => x.id == orgCode)?.name
    this.orgName = orgName
  }
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    const formData = new FormData()
    formData.append('file', item.file as any)
    formData.append('referenceId', this.opinionDetail.fileId || '')
    let params = new HttpParams()
    if (this.opinionDetail.fileId) {
      params = params.set('referenceId', this.opinionDetail.fileId)
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

  handleChange({ file, fileList }: NzUploadChangeParam): void {
    const status = file.status
    if (status !== 'uploading') {
    }
    if (status === 'done') {
      this.msg.success(`${file.name} file uploaded successfully.`)
      this.fileList = fileList
    } else if (status === 'error') {
      this.msg.error(`${file.name} file upload failed.`)
    }
  }
  changeStatusOpinion(status: string) {
    if (status != '-') {
      this.opinionDetail.status = status
    }
    this.opinionDetail.action = status
    this.opinionDetail.statusName =
      status == '01'
        ? 'Khởi tạo'
        : status == '02'
          ? 'Chờ phê duyệt'
          : status == '03'
            ? 'Đã phê duyệt'
            : status == '04'
              ? 'Từ chối'
              : status == '05'
                ? ' Đã hoàn thành'
                : 'Chưa hoàn thành'
    this.updateOpinionDetail()
  }
  updateOpinionDetail() {
    if(this.opinionDetail.status == '03' && this.opinionDetail.action == '03'){
      this.opinionDetail.contentReport = this.opinionDetail.contentOrg 
      this._serviceOpinionList
      .UpdateOpinionDetail(this.opinionDetail)
      .subscribe((res: any) => {
        this.lstHistoryOpinionDetail = res
        this.opinionDetail.textContent = ''
      })
    }else{
     // this.opinionDetail.contentReport = this.textContent;
      this._serviceOpinionList
      .UpdateOpinionDetail(this.opinionDetail)
      .subscribe((res: any) => {
        this.lstHistoryOpinionDetail = res
        this.opinionDetail.textContent = ''
      })
    }
  }

  showModal(status: string) {
    if (status != '-') {
      this.opinionDetail.status = status
    }
    this.opinionDetail.action = status
    this.opinionDetail.statusName =
      status == '01'
        ? 'Khởi tạo'
        : status == '02'
          ? 'Chờ phê duyệt'
          : status == '03'
            ? 'Đã phê duyệt'
            : status == '04'
              ? 'Từ chối'
              : status == '05'
                ? ' Đã hoàn thành'
                : 'Chưa hoàn thành'
    this.isVisibleModal = true
  }
  getModalMessage() {
    switch (this.opinionDetail.status) {  
      case '01':
        return 'Bạn có chắc chắn yêu cầu chỉnh sửa thông tin này?';
      case '02':
        return 'Bạn có chắc chắn trình duyệt thông tin này?';
      case '03':
        return 'Bạn có chắc chắn phê duyệt thông tin này?';
      case '04':
        return 'Bạn có chắc chắn từ chối thông tin này?';
      default:
        return '';
    }
  }
  getModalAuditMessage() {
    switch (this.status) {  
      case '01':
        return 'Bạn có chắc chắn yêu cầu chỉnh sửa thông tin này?';
      case '02':
        return 'Bạn có chắc chắn trình duyệt thông tin này?';
      case '03':
        return 'Bạn có chắc chắn phê duyệt thông tin này?';
      case '04':
        return 'Bạn có chắc chắn từ chối thông tin này?';
      default:
        return '';
    }
  }

  changeStatusAudit(status: string) {
    if (status != '-') {
      this.status = status
    }
    this.validateForm.getRawValue().status = status
    this.validateForm.getRawValue().status =
      status == '01'
        ? 'Khởi tạo'
        : status == '02'
          ? 'Chờ phê duyệt'
          : status == '03'
            ? 'Đã phê duyệt'
            : 'Từ chối'
    this.updateAudit()
  }
  updateAudit() {
    //const statusCode = this.status === 'Khởi tạo' ? '01' : this.status === 'Chờ xác nhận' ? '02' : this.status === 'Đã phê duyệt' ? '03' : '04';
    const formData = {
      ...this.validateForm.value,
      code: this.validateForm.get('code')?.value,
      status: this.status,
      textContent: this.textContent,
    }
    this._service.UpdateAudit(formData).subscribe((res: any) => {
      this.listAuditHistory = res
      this._service
        .getListAuditByCode(this.validateForm.getRawValue().code)
        .subscribe((data) => {
          this.populateListAuditForm(data)
          this.status = data.status
        })
    })
  }

  showAuditModal(status: string) {
    if (status != '-') {
      this.status = status
    }
    this.validateForm.getRawValue().status = status
    //console.log('abc', status)
    //this.validateForm.getRawValue().status = status == "01" ? "Khởi tạo" : status == "02" ? "Chờ phê duyệt" : status == "03" ? "Đã phê duyệt" : "Từ chối";
    this.isVisibleAuditModal = true
  }

  handleAuditOk() {
    this.updateAudit()
    this.isVisibleAuditModal = false
  }

  handleAuditCancel() {
    this.validateForm.getRawValue().textContent = ''
    this.isVisibleAuditModal = false
  }

  handleOk() {
    this.updateOpinionDetail()
    this.isVisibleModal = false
  }

  handleCancel() {
    this.opinionDetail.textContent = ''
    this.isVisibleModal = false
  }
  getNodeOrg() {
    if(this.userType == 'ACT_STC'){
      this._service.getListOrg(this.code).subscribe((res) => {
        this.nodeOrg = [res]
      })  
    }else{
      this._serviceOrg.GetOrgTreeForUser(this.userName).subscribe((res) => {
        this.nodeOrg = [res]
      })  
    }    
  }
  getNodeOpinion() {
    this._serviceOpinionList
      .GetOplTreeWithMgCode(this.opinionCodeTree)
      .subscribe((res) => {
        this.nodeOpinion = [res]
      })
  }
  nzEvent(event: NzFormatEmitEvent): void {
    console.log(event)
  }
  nzCheck(event: NzFormatEmitEvent): void {
    console.log(event)
  }

  onClickNodeOrg(node: any) {
    this.visible = true
    this.nodeCurrentOrg = node?.origin
    this._serviceOpinionList
      .GetOplTreeWithMgCodeAndOrg(this.nodeCurrentOrg.id, this.opinionCodeTree)
      .subscribe((res) => {
        this.nodeOpinion = [res]
        console.log(this.nodeOpinion)
      })
  }
  onClickNodeOpinion(node: any) {
    this.visibleEditOpinion = true
    this.nodeCurrentOpinion = node?.origin

    this._serviceOpinionList
      .GetOpinionDetail(
        this.nodeCurrentOrg.id,
        this.auditCode,
        this.nodeCurrentOpinion.id,
      )
      .subscribe((res) => {
        this.opinionDetail = res
        this.lstHistoryOpinionDetail = res.history
        this.opinionDetail.statusName =
          this.opinionDetail.status == '01'
            ? 'Khởi tạo'
            : this.opinionDetail.status == '02'
              ? 'Chờ phê duyệt'
              : this.opinionDetail.status == '03'
                ? 'Đã phê duyệt'
                : this.opinionDetail.status == '04'
                  ? 'Từ chối'
                  : this.opinionDetail.status == '05'
                    ? ' Đã hoàn thành'
                    : 'Chưa hoàn thành'
        this.loadAttachmentsOpinion()
      })
  }
  closeEditOpinion() {
    this.visibleEditOpinion = false
  }
  getOpinion() {
    this._mgops.getall().subscribe((res) => {
      this.opinionList = res
      // lấy lại danh sách cây
    })
  }
  getTimeYear() {
    this.dropDownService.getAllPeriodTime().subscribe({
      next: (data) => {
        this.listTimeYear = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAudit() {
    this.dropDownService.getAllAudit().subscribe({
      next: (data) => {
        this.listAudit = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getRightOfUser(userName: string){
    this._rightService.GetRightOfUser(this.userName).subscribe({
      next: (data : any) => {  
        this.CheckRightOfUser(data, "RP1.1.7")
        this.checkStatus()
      }
    }
    )
  }
  CheckRightOfUser(data: any, rightsToCheck: string){
    // const result: { [key: string]: boolean } = {};
    // for (const rightsToCheck of data) {
    //   result[rightsToCheck] = data.includes(rightsToCheck);
    // }
    // if (result){
    //   this.checkRightEdit = true;
    // }   
    // return result;
    data.forEach((item : any) => {
      if (item == rightsToCheck) {
        this.checkRightEdit = true;
        return;
      }
    })
  }

  // submitForApproval() {
  //   this._service.submitForApproval(this.validateForm.getRawValue().code).subscribe(
  //     () => {
  //       this._service.getListAuditByCode(this.validateForm.getRawValue().code).subscribe(data => {
  //         this.populateListAuditForm(data);
  //         this.status = data.status;
  //       });
  //     },
  //     error => console.error('Error submitting for approval', error)
  //   );
  // }

  // submitToCancel() {
  //   this._service.submitToCancel(this.validateForm.getRawValue().code).subscribe(
  //     () => {
  //       this._service.getListAuditByCode(this.validateForm.getRawValue().code).subscribe(data => {
  //         this.populateListAuditForm(data);
  //         this.status = data.status;
  //       });
  //     },
  //     error => console.error('Error submitting for approval', error)
  //   );
  // }

  // approveAudit() {
  //   this._service.approveAudit(this.validateForm.getRawValue().code).subscribe(
  //     () => {
  //       this._service.getListAuditByCode(this.validateForm.getRawValue().code).subscribe(data => {
  //         this.populateListAuditForm(data);
  //         this.status = data.status;
  //       });
  //     },
  //     error => console.error('Error approving audit', error)
  //   );
  // }

  // rejectAudit() {
  //   this._service.rejectAudit(this.validateForm.getRawValue().code).subscribe(
  //     () => {
  //       this._service.getListAuditByCode(this.validateForm.getRawValue().code).subscribe(data => {
  //         this.populateListAuditForm(data);
  //         this.status = data.status;
  //       });
  //     },
  //     error => console.error('Error rejecting audit', error)
  //   );
  // }

  submitFormUpdate() {
    if (this.validateForm.valid) {
      const statusCode =
        this.status === 'Khởi tạo'
          ? '01'
          : this.status === 'Chờ xác nhận'
            ? '02'
            : this.status === 'Đã phê duyệt'
              ? '03'
              : '04'
      const formData = {
        ...this.validateForm.value,
        code: this.validateForm.get('code')?.value,
        status: statusCode,
      }
      this._service.updateListAudit(formData).subscribe({
        next: (data) => { },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }
  loadAttachments() {
    if (this.fileId) {
      this._service.getListFile(this.fileId).subscribe({
        next: (response: any[]) => {
          this.attachments = response.map((item) => item.attachment)
        },
        error: (error) => {
          console.error('Lỗi khi tải danh sách file đính kèm', error)
          this.message.error('Không thể tải danh sách file đính kèm')
        },
      })
    }
  }
  loadAttachmentsOpinion() {
    if (this.opinionDetail.fileId) {
      this._service.getListFile(this.opinionDetail.fileId).subscribe({
        next: (response: any[]) => {
          this.attachmentsOpinion = response.map((item) => item.attachment)
          console.log(this.attachmentsOpinion)
        },
        error: (error) => {
          console.error('Lỗi khi tải danh sách file đính kèm', error)
          this.message.error('Không thể tải danh sách file đính kèm')
        },
      })
    }
  }

  downloadFile(file: any): void {
    // Implement file download logic here
    console.log('Downloading file:', file)
  }
  checkStatus() {
    if (this.status === 'Đã phê duyệt' || this.checkRightEdit == false) {
      this.validateForm.disable();  // Disable toàn bộ form
    } else {
      this.validateForm.enable();   // Enable lại form nếu cần
    }
  }

  deleteFile(file: any): void {
    // Implement file deletion logic here
    console.log('Deleting file:', file)
    // After successful deletion, reload attachments
    this.loadAttachments()
  }

  resetForm() {
    this.validateForm.reset()
  }
  close() {
    this.visible = false
    this.resetForm()
  }
}
