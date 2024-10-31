import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CalculateResultService {
  constructor(private commonService: CommonService) {}

  GetResult(code : any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetCalculateResult?code=${code}`)
  }
  GetDataInput(code : any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetDataInput?code=${code}`)
  }
  UpdateDataInput(model : any): Observable<any> {
    return this.commonService.post(`CalculateResult/UpdateDataInput`, model)
  }
  GetHistoryAction(code : any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetHistoryAction?code=${code}`)
  }
  ExportExcel(headerId: any): Observable<any> {
    return this.commonService.downloadFile(`CalculateResult/ExportExcel?headerId=${headerId}`)
  }
}
