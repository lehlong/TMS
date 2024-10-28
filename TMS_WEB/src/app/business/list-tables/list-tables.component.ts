import { ShareModule } from './../../shared/share-module/index'
import {
  Component,
  EventEmitter,
  Input,
  NgZone,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core'
import {
  NzFormatEmitEvent,
  NzTreeComponent,
  NzTreeNodeOptions,
} from 'ng-zorro-antd/tree'
import { ListTablesService } from '../../services/business/list-table.service'
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms'
import { GlobalService } from '../../services/global.service'
import { AuditPeriodFilter } from '../../models/master-data/audit-period.model'
import { PeriodTimeFilter } from '../../models/master-data/period-time.model'
import { MgListTablesFilter } from '../../models/master-data/mg-list-tables.model'
import { MgListTablesService } from '../../services/master-data/mg-list-tables.service'
import { PeriodTimeService } from '../../services/master-data/period-time.service'
import { AuditPeriodService } from '../../services/master-data/audit-period.service'
import { ActivatedRoute, Router } from '@angular/router'
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http'
import { Subscription } from 'rxjs'
import { tap } from 'rxjs/operators'
import { NzMessageService } from 'ng-zorro-antd/message'
import { v4 as uuidv4 } from 'uuid'
import { DropdownService } from '../../services/dropdown/dropdown.service'
import { NzUploadChangeParam, NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload'
import { environment } from '../../../environments/environment'

@Component({
  selector: 'app-list-tables',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './list-tables.component.html',
  styleUrl: './list-tables.component.scss',
})
export class ListTablesComponent implements OnInit {
  mgCode: string = ''
  groupCode: string = '';
  mgData: any
  @ViewChild('treeCom', { static: false }) treeCom!: NzTreeComponent
  auditPeriodfilter = new AuditPeriodFilter()
  periodTimefilter = new PeriodTimeFilter()
  filter = new MgListTablesFilter()
  timeyear: any[] = []
  auditPeriod: any[] = []
  searchValue = ''
  nodes: any = []
  originalNodes: any[] = []
  currency: any[] = []
  isUploadModalVisible = false
  isUploading = false
  fileToUpload: File | null = null
  fileList: NzUploadFile[] = []
  referenceId: string = ''
  visible: boolean = false
  edit: boolean = false
  nodeCurrent!: any
  titleParent: string = ''
  loading: boolean = false
  templateFileUrl = 'assets/templates/ACM_Template_Cay_danh_muc_bang_bieu.xlsx'
  public baseUrl = environment.baseApiUrl
  validateForm: FormGroup = this.fb.group({
    code: [''],
    id: ['', [Validators.required]],
    name: ['', [Validators.required]],
    pId: ['', [Validators.required]],
    currencyCode: ['', [Validators.required]],
    children: [null],
    orderNumber: [null],
    mgCode: [''],
  })
  mgListTableForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    name: ['', [Validators.required]],
    description: [''],
    timeYear: ['', [Validators.required]],
    auditPeriod: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
    groupCode: [''],
  })

  constructor(
    private _service: ListTablesService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private _mgs: MgListTablesService,
    private _ps: PeriodTimeService,
    private _ap: AuditPeriodService,
    private dropdownService: DropdownService,
    private route: ActivatedRoute,
    private router: Router,
    private message: NzMessageService,
    private http: HttpClient,
    private ngZone: NgZone
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách bảng biểu',
        path: 'master-data/mg-list-tables',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }
  private populateMgListTableForm(data: any) {
    this.mgListTableForm.patchValue({
      code: data.code,
      name: data.name,
      description: data.description,
      timeYear: data.timeYear,
      auditPeriod: data.auditPeriod,
      isActive: data.isActive,
      groupCode: data.groupCode,
    })
  }
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.mgCode = params['code']
      const navigation = this.router.getCurrentNavigation()
      if (navigation?.extras.state) {
        const state = navigation.extras.state as { mgListTableData: any }
        if (state.mgListTableData) {
          this.populateMgListTableForm(state.mgListTableData)
        }
      }
      this.getLtbWithMgCode()
      this.getAllAuditPeriod()
      this.getAllPeriodTime()
      this.getCurrency()
      this.getMgData()
    })
  }
  getMgData() {
    if (this.mgListTableForm.get('code')?.value) {
      // If the form is already populated, don't fetch data again
      return
    }
    this._mgs.searchMgListTables({ KeyWord: this.mgCode }).subscribe({
      next: (data) => {
        if (data.data && data.data.length > 0) {
          this.mgData = data.data[0]
          this.populateMgListTableForm(this.mgData)
        }
      },
      error: (error) => console.error(error),
    })
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['mgCode'] && this.mgCode) {
      this.getLtbWithMgCode()
    }
  }

  getLtbWithMgCode() {
    this._service.GetLtbTreeWithMgCode(this.mgCode).subscribe((res) => {
      this.nodes = [res]
      this.originalNodes = [res]
      this.expandAllNodes(this.nodes);
    })
  }
  expandAllNodes(nodes: NzTreeNodeOptions[]): void {
    nodes.forEach(node => {
      node.expanded = true;
      if (node.children && node.children.length > 0) {
        this.expandAllNodes(node.children);
      }
    });
  }
  getCurrency() {
    this.dropdownService
      .getAllCurrency({
        IsCustomer: true,
        SortColumn: 'name',
        IsDescending: true,
      })
      .subscribe({
        next: (data) => {
          this.currency = data
        },
        error: (response) => {
          console.log(response)
        },
      })
  }
  nzEvent(event: NzFormatEmitEvent): void {
    // console.log(event);
  }

  onDrop(event: any) { }

  onDragStart(event: any) { }
  downloadTemplate() {
    const link = document.createElement('a');
    link.setAttribute('target', '_blank');
    link.setAttribute('href', this.templateFileUrl);
    link.setAttribute('download', 'ACM_Template_Cay_danh_muc_bang_bieu.xlsx');
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
  onClick(node: any) {
    this.edit = true
    this.visible = true
    this.nodeCurrent = node?.origin
    this.titleParent = node.parentNode?.origin?.title || ''
    this.validateForm.setValue({
      code: this.nodeCurrent?.code,
      id: this.nodeCurrent?.id,
      name: this.nodeCurrent?.name,
      pId: this.nodeCurrent?.pId,
      children: [],
      orderNumber: this.nodeCurrent?.orderNumber,
      mgCode: this.nodeCurrent?.mgCode || this.mgCode,
      currencyCode: this.nodeCurrent?.currencyCode,
    })
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.searchValue = ''
    this.getLtbWithMgCode()
    this.nodes = [...this.originalNodes]
  }

  resetForm() {
    this.validateForm.reset()
  }

  openCreateChild(node: any) {
    this.close()
    this.edit = false
    this.visible = true
    this.validateForm.get('pId')?.setValue(node?.origin.id)
    this.validateForm.get('orderNumber')?.setValue(null)
    this.validateForm.get('children')?.setValue([])
    this.validateForm.get('mgCode')?.setValue(this.mgCode)
    this.validateForm.get('currencyCode')?.setValue('VND')
  }

  openCreate() {
    this.close()
    this.edit = false
    this.visible = true
    this.validateForm.get('pId')?.setValue(this.nodeCurrent?.id || 'R')
    this.validateForm.get('children')?.setValue([])
    this.validateForm.get('orderNumber')?.setValue(null)
    this.validateForm.get('mgCode')?.setValue(this.mgCode)
    this.validateForm.get('currencyCode')?.setValue('VND')
  }
  isIdExist(id: string, node: any): boolean {
    if (node.id === id) {
      return true
    }
    if (node.children) {
      for (const child of node.children) {
        if (this.isIdExist(id, child)) {
          return true
        }
      }
    }
    return false
  }
  importExcelAndUpdateData(file: File) {

    this._service.uploadExcel(file, this.mgCode).subscribe({
      next: (result) => {
        console.log('Data imported successfully', result)
        this.getLtbWithMgCode()
        // Refresh data after import
      },
      error: (error) => {
        console.error('Error importing data:', error)
      },
    })

  }
  submitForm() {
    if (!this.validateForm.valid) {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty()
          control.updateValueAndValidity({ onlySelf: true })
        }
      })
      return
    }
    const formData = this.validateForm.getRawValue()
    console.log('Form data', formData)

    if (this.edit) {
      this._service.Update(formData).subscribe({
        next: (data) => {
          this.getLtbWithMgCode()
        },
        error: (response) => {
          console.log(response)
        },
      })
    } else {
      const newId = formData.id
      const idExists = this.nodes.some((node: any) =>
        this.isIdExist(newId, node),
      )
      if (idExists) {
        this.message.error(
          `Mã danh mục ${newId} đã được sử dụng, vui lòng nhập lại`,
        )
        return
      }
      formData.code = uuidv4()
      this._service.Insert(formData).subscribe({
        next: (data) => {
          this.getLtbWithMgCode()
          console.log(data)
        },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }
  submitFormUpdate() {
    this.ngZone.run(() => {
      if (this.mgListTableForm.valid) {
        const formData = this.mgListTableForm.value;
        this._mgs.updateMgListTables(formData).subscribe({
          next: (data) => {
            console.log("formData", formData);
            this.getMgData();
          },
          error: (response) => {
            console.log(response);
          },
        });
      }
    });
  }
  updateOrderTree() {
    const treeData = this.treeCom
      .getTreeNodes()
      .map((node) => this.mapNode(node))
    const updatedTreeData = {
      ...treeData[0],
      mgCode: this.mgCode,
    }
    this._service.UpdateOrderTree(updatedTreeData).subscribe({
      next: (data) => {
        this.getLtbWithMgCode()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  private mapNode(node: any): any {
    const children = node.children
      ? node.children.map((child: any) => this.mapNode(child))
      : []
    return {
      code: node.origin.code,
      id: node.origin.id,
      pId: node.parentNode?.key || '-',
      name: node.origin.name,
      children: children,
      mgCode: this.mgCode,
      currencyCode: node.origin.currencyCode,
    }
  }

  deleteItem(node: any) {
    if (node.children && node.children.length > 0) {
      // Thông báo rằng không thể xóa vì node có children
      this.message.error(
        'Không được phép xóa Cấu trúc tổ chức Cha khi còn các thành phần con',
      )
      return // Dừng quá trình xóa
    }
    this._service.Delete(node.origin.code).subscribe({
      next: (data) => {
        this.getLtbWithMgCode()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  searchTables(searchValue: string) {
    const filterNode = (node: NzTreeNodeOptions): boolean => {
      if (node.title.toLowerCase().includes(searchValue.toLowerCase())) {
        return true
      } else if (node.children) {
        node.children = node.children.filter((child) => filterNode(child))
        return node.children.length > 0
      }
      return false
    }

    if (!searchValue) {
      this.nodes = [...this.originalNodes]
    } else {
      this.nodes = this.originalNodes
        .map((node) => ({ ...node }))
        .filter((node) => filterNode(node))
    }
  }
  getAllAuditPeriod() {
    this._ap.searchAuditPeriod(this.auditPeriodfilter).subscribe({
      next: ({ data }) => {
        this.auditPeriod = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAllPeriodTime() {
    this._ps.searchPeriodTime(this.periodTimefilter).subscribe({
      next: ({ data }) => {
        this.timeyear = data
      },
      error: (response) => {
        console.log(response)
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
  beforeUpload = (file: NzUploadFile): boolean => {
    const isXlsx =
      file.type ===
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    const isXls = file.type === 'application/vnd.ms-excel' // MIME type cho file .xls
    if (!isXlsx && !isXls) {
      this.message.error('Sai định dạng file. Vui lòng cho file .xlsx hoặc .xls')
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
      this.message.success(`File ${file.name} upload thành công.`)
    } else if (status === 'error') {
      this.fileToUpload = null
      this.message.error(`File ${file.name} upload thất bại.`)
    } else if (status === 'removed') {
      this.fileToUpload = null
      this.message.remove(`Đã xóa file ${file.name}.`)
    }
    this.fileList = fileList
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
}
