import { Component, VERSION } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { DropdownService } from '../../services/dropdown/dropdown.service';
import { NzUploadChangeParam, NzUploadFile, NzUploadModule, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { SafeResourceUrl, DomSanitizer } from "@angular/platform-browser";
import { environment } from '../../../environments/environment.prod';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Subscription } from 'rxjs';
import { HttpClient, HttpEventType, HttpParams } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import { v4 as uuidv4 } from 'uuid';
import { ReportService } from '../../services/report/report.service';
@Component({
  selector: 'app-report',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss'
})
export class ReportComponent {
  templateFileUrl = 'assets/templates/Báo_Cáo_79.docx'
  listTimeYear: any[] = [];
  listAudit: any[] = [];
  public baseUrl = environment.baseApiUrl
  fileList: NzUploadFile[] = [];
  referenceId: string = '';
  yearValue: string = ''
  auditValue: string = ''
  urlSafe: any;
  reportUrl: string = `https://docs.google.com/gview?url=http://sso.d2s.com.vn:5001/assets/templates/BaoCao79.docx&embedded=true`
  urlDownload: string =''
  listElement: any[] = []
  idTemplate: string = ''
  constructor(
    private dropDownService: DropdownService,
    public sanitizer: DomSanitizer,
    private msg: NzMessageService,
    private http: HttpClient,
    private _service: ReportService
  ) { }
  ngOnInit(): void {
    this.getTimeYear();
    this.getAuditPeriod();
    this.generateNewReferenceId();
    // if(this.reportUrl != ''){
    //   this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.reportUrl);
    // }
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
  getAuditPeriod() {
    this.dropDownService.getAllAudit().subscribe({
      next: (data) => {
        this.listAudit = data
      },
      error: (response) => {
        console.log(response)
      }
    })
  }
  viewTemplate(){
    if(this.yearValue != '' && this.auditValue != ''){
      this.getListTemplate(this.yearValue, this.auditValue)
    }
  }
  getListTemplate(yearValue: string, auditValue: string) {
    if(this.idTemplate != ''){
      this._service.getTemplate(this.idTemplate, this.yearValue, this.auditValue).subscribe({
        next: (data) => {

        }
      })
    }else{
      this._service.getListTemplate(yearValue, auditValue).subscribe({
        next: (data) => {
          this.listElement = data
        },
        error: (response) => {
          console.log(response)
        }
      })
    }
  }


  downloadTemplate() {
    const link = document.createElement('a');
    link.setAttribute('target', '_blank');
    link.setAttribute('href', this.templateFileUrl);
    link.setAttribute('download', 'Báo_Cáo_79.docx');
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
  generateNewReferenceId(): void {
    this.referenceId = uuidv4();
  }
  customRequest = (item: NzUploadXHRArgs): Subscription => {
    const formData = new FormData();
    formData.append('file', item.file as any);
    formData.append('referenceId', this.referenceId || '');
    formData.append('yearValue', this.yearValue || '');
    formData.append('auditValue', this.auditValue || '');
    let params = new HttpParams();
    if (this.referenceId && this.yearValue && this.auditValue) {
        params = params.set('referenceId', this.referenceId);
        params = params.set('yearValue', this.yearValue);
        params = params.set('auditValue', this.auditValue);
    }
    return this.http.post(this.baseUrl + '/Report/Upload', formData, {
      params: params,
    }).subscribe({
          next:(res: any) =>{
            this.reportUrl = `https://docs.google.com/gview?url=${environment.baseApiUrl}/${res.data}&embedded=true`
            console.log(this.reportUrl)
            if(res.data != ''){
              this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.reportUrl);
            }
            this.urlDownload = `${environment.baseApiUrl}/${res.data}`
          },
        });
  }
  handleChange({ file, fileList }: NzUploadChangeParam): void {
    const status = file.status;
    if (status !== 'uploading') {
    }
    if (status === 'done') {
      //this.msg.success(`${file.name} đã tải lên thành.`);
      this.fileList = fileList;
    } else if (status === 'error') {
      //this.msg.error(`${file.name} file upload failed.`);
    }
  }
}
