import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  constructor(private commonService: CommonService) { }

  upload(params: any): Observable<any> {
    return this.commonService.post('Report/Upload', params)
  }
  getListTemplate(yearValue: string, auditValue: string): Observable<any> {
    return this.commonService.get(`Report/GetListTemplate/${yearValue}/${auditValue}`)
  }
  getTemplate(id: string, year: string, audit: string): Observable<any> {
    return this.commonService.get(`Report/GetTemplate/${id}/${year}/${audit}`)
  }
  getListElement(fileId: string): Observable<any> {
    return this.commonService.get(`Report/GetListElement/${fileId}`)
  }
  SaveTemplateReport(params: any): Observable<any> {
    return this.commonService.post('Report/SaveTemplateReport', params)
  }
  getListOrg(fileId: string, textElement: string): Observable<any>{
    return this.commonService.get('Report/GetListOrg', {fileId, textElement})
  }
  getListOpinion(params: any): Observable<any>{
    return this.commonService.get('Report/GetListOpinion', params)
  }
}
