import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { ListAuditFilter } from '../../models/master-data/list-audit.model';
import { GlobalService } from '../../services/global.service';
import { ListAuditService } from '../../services/master-data/list-audit.service';
import { PaginationResult } from '../../models/base.model';
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms';
import { LIST_AUDIT_RIGHTS } from '../../shared/constants';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzUploadChangeParam, NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { DropdownService } from '../../services/dropdown/dropdown.service';
import { MgOpinionListService } from '../../services/master-data/mg-opinion-list.service';
import { environment } from '../../../environments/environment';
import { v4 as uuidv4 } from 'uuid';
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
@Component({
  selector: 'app-list-audit',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './list-audit.component.html',
  styleUrl: './list-audit.component.scss'
})
export class ListAuditComponent {
  public baseUrl = environment.baseApiUrl

  edit: boolean = false;
  listUser: any[] = [];
  validateForm: FormGroup = this.fb.group({
    code: ['', [Validators.required]],
    name: ['', [Validators.required]],
    timeYear: ['', [Validators.required]],
    auditPeriod: ['', [Validators.required]],
    reportDate: ['', [Validators.required]],
    reportNumber: ['', [Validators.required]],
    status: [{ value: this.edit ? '' : 'Khởi tạo', disabled: !this.edit }, [Validators.required]], // Mặc định trạng thái là "Khởi tạo"
    startDate: ['', [Validators.required]],
    endDate: ['', [Validators.required]],
    note: [''],
    opinionCode: ['', [Validators.required]],
    fileId: [''],
    textContent: [''],
    approver: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  });
  status: string = '';
  isSubmit: boolean = false;
  visible: boolean = false;
  filter = new ListAuditFilter();
  paginationResult = new PaginationResult();
  loading: boolean = false;
  opinionList: any[] = [];
  listTimeYear: any[] = [];
  listAudit: any[] = []
  availableYears: any[] = [];
  availablePeriods: any[] = [];
  LIST_AUDIT_RIGHTS = LIST_AUDIT_RIGHTS;
  referenceId: string = '';
  fileList: NzUploadFile[] = [];

  showStatistic: boolean = false;
  opinionStatistic: any
  opinionStatisticSearch: any
  orgs: any
  searchValue = '';
  constructor(
    private _service: ListAuditService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private msg: NzMessageService,
    private _mgops: MgOpinionListService,
    private dropDownService: DropdownService,
    private http: HttpClient,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách đơn vị tính',
        path: 'master-data/unit',
      },
    ]);
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([]);
  }

  ngOnInit(): void {
    this.search();
    this.getOpinion();
    this.getTimeYear();
    this.generateNewReferenceId();
    this.getAudit();
    this.getUser();
    this.getAllOrg();
    this.loadYearsAndPeriods();
  }
  getUser() {
    this.dropDownService.GetAllAccount().subscribe((res) => {
      this.listUser = res;
    })
  }
  getAllOrg() {
    this.dropDownService.getAllOrg().subscribe((res) => {
      this.orgs = res;
    })
  }
  getOrgName(data: string) {
    //  const matchedAccount = this.account.find((acc) => acc.userName === createBy)
    if (data) {
      const matchedOrg = this.orgs.find(
        (org: { id: any }) => org.id === data,
      )
      if (matchedOrg) {
        return matchedOrg.name
      }
    }
    return 'N/A'
  }

  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    };
    this.search();
  }
  getOpinion() {
    this._mgops.getall().subscribe((res) => {
      this.opinionList = res
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
      }
    })
  }
  loadYearsAndPeriods() {
    this.dropDownService.getAllPeriodTime().subscribe(years => {
      this.listTimeYear = years;
      this.updateAvailableYears();
    });

    this.dropDownService.getAllAudit().subscribe(periods => {
      this.listAudit = periods;
    });
  }
  updateAvailableYears() {
    this._service.searchListAudit(new ListAuditFilter()).subscribe(result => {
      const usedYearPeriods = result.data.map((item: { timeYear: any; auditPeriod: any }) => ({ year: item.timeYear, period: item.auditPeriod }));

      this.availableYears = this.listAudit.filter(year => {
        const periodsForYear = usedYearPeriods.filter((up: { year: any }) => up.year === year.timeyear);
        return periodsForYear.length < 2; // Year is available if it has less than 2 periods used
      });

      if (this.validateForm.get('timeYear')?.value) {
        this.updateAvailablePeriods();
      }
    });
  }

  updateAvailablePeriods() {
    const selectedYear = this.validateForm.get('timeYear')?.value;
    this._service.searchListAudit(new ListAuditFilter()).subscribe(result => {
      const usedPeriods = result.data
        .filter((item: { timeYear: any }) => item.timeYear === selectedYear)
        .map((item: { auditPeriod: any }) => item.auditPeriod);

      this.availablePeriods = this.listAudit.filter(period => !usedPeriods.includes(period.code));
    });
  }
  onYearChange() {
    this.validateForm.patchValue({ auditPeriod: null });
    this.updateAvailablePeriods();
  }
  search() {
    this.isSubmit = false;
    this._service.searchListAudit(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data;
        this.status = data.data.status;
      },
      error: (response) => {
        console.log(response);
      },
    });
  }

  exportExcel() {
    return this._service.exportExcelListAudit(this.filter).subscribe((result: Blob) => {
      const blob = new Blob([result], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const url = window.URL.createObjectURL(blob);
      var anchor = document.createElement('a');
      anchor.download = 'danh-sach-dia-phuong.xlsx';
      anchor.href = url;
      anchor.click();
    });
  }
  generateNewReferenceId(): void {
    this.referenceId = uuidv4();
    this.validateForm.patchValue({ fileId: this.referenceId });
  }
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    const formData = new FormData();
    formData.append('file', item.file as any);
    formData.append('referenceId', this.referenceId || '');
    let params = new HttpParams();
    if (this.referenceId) {
      params = params.set('referenceId', this.referenceId);
    }
    return this.http.post(this.baseUrl + '/ModuleAttachment/Upload', formData, {
      params: params,
      reportProgress: true,
      observe: 'events'
    }).pipe(
      tap(event => {
        if (event.type === HttpEventType.UploadProgress && item.onProgress) {
          item.onProgress(event, item.file);
        }
        if (event.type === HttpEventType.Response && item.onSuccess) {
          item.onSuccess(event.body, item.file, this.fileList);
        }
      })
    ).subscribe({
      error: (err) => {
        if (item.onError) {
          item.onError(err, item.file);
        } else {
          console.error('onError function is not defined');
        }
      }
    });
  }

  submitForm(): void {
    this.isSubmit = true;
    if (this.validateForm.valid) {
      this.validateForm.patchValue({
        fileId: this.referenceId,
        status: '01'
      });
      const formData = this.validateForm.getRawValue();
      formData.fileList = this.fileList.map(file => file.response?.url || '');

      if (this.edit) {
        this._service.updateListAudit(this.validateForm.getRawValue()).subscribe({
          next: (data) => {
            this.search();
          },
          error: (response) => {
            console.log(response);
          },
        });
      } else {
        console.log(this.validateForm.getRawValue())
        this._service.createListAudit(this.validateForm.getRawValue()).subscribe({
          next: (data) => {
            this.search();
          },
          error: (response) => {
            console.log(response);
          },
        });
      }
    } else {
      Object.values(this.validateForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
  handleChange({ file, fileList }: NzUploadChangeParam): void {
    const status = file.status;
    if (status !== 'uploading') {
    }
    if (status === 'done') {
      this.msg.success(`${file.name} file uploaded successfully.`);
      this.fileList = fileList;
    } else if (status === 'error') {
      this.msg.error(`${file.name} file upload failed.`);
    }
  }

  close() {
    this.visible = false;
    this.resetForm();
  }

  reset() {
    this.filter = new ListAuditFilter();
    this.search();
  }

  openCreate() {
    this.edit = false;
    this.visible = true;
    this.generateNewReferenceId();
    this.fileList = [];

  }

  resetForm() {
    this.validateForm.reset({ isActive: true });
    this.isSubmit = false;
    this.fileList = [];
    this.generateNewReferenceId();
  }

  deleteItem(code: string | number) {
    this._service.deleteListAudit(code).subscribe({
      next: (data) => {
        this.search();
      },
      error: (response) => {
        console.log(response);
      },
    });
  }

  showModalStatistic(code: string) {
    this._service.getOpinionStatis(code).subscribe({
      next: (data) => {
        this.opinionStatistic = data
        this.opinionStatisticSearch = data
        this.opinionStatistic.sort((a: any, b: any) => {
          if (a.orgCode === b.orgCode) {
            // Nếu orgCode giống nhau, sắp xếp theo opinion
            if (a.opinion < b.opinion) return -1;
            if (a.opinion > b.opinion) return 1;
            return 0;
          }
          // Sắp xếp theo orgCode
          if (a.orgCode < b.orgCode) return -1;
          if (a.orgCode > b.orgCode) return 1;
          return 0;
        });
        this.showStatistic = true
      },
      error: (response) => {
        console.log(response);
      },
    });
  }
  handleOk(): void {
    setTimeout(() => {
      this.showStatistic = false;
    }, 30);
  }

  handleCancel(): void {
    this.showStatistic = false;
  }
  openEdit(data: { code: string }) {
    this._service.getListAuditHistory(data.code).subscribe({
      next: (result) => {
        if (result) { // Log toàn bộ kết quả trả về từ API
          this.router.navigate(['/master-data/list-audit-edit', data.code], {
            state: { ListAuditData: result },  // Pass the whole object
          });
        } else {
          console.warn("No data found for the given code.");
        }
      },
      error: (error) => {
        console.error("API call failed: ", error);  // Log chi tiết lỗi
      },
    });
  }
  onSearch(): void {

    this.opinionStatistic = this.searchValue == '' || this.searchValue == null ? this.opinionStatisticSearch : this.opinionStatistic.filter((data: { orgCode: string; opinion: string; status: string; }) =>
      this.getOrgName(data.orgCode).toLowerCase().includes(this.searchValue.toLowerCase()) ||
      data.opinion.toLowerCase().includes(this.searchValue.toLowerCase()) ||
      data.status.toLowerCase().includes(this.searchValue.toLowerCase())
    );

  }
  resetSearch() {
    this.searchValue = ''
  }


  pageSizeChange(size: number): void {
    this.filter.currentPage = 1;
    this.filter.pageSize = size;
    this.search();
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index;
    this.search();
  }
}
