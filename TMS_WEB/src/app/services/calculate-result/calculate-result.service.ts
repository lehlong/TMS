import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CalculateResultService {
  constructor(private commonService: CommonService) {}

  GetResult(code : any, tab : any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetCalculateResult?code=${code}&tab=${tab}`)
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
  GetHistoryFile(code : any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetHistoryFile?code=${code}`)
  }
  ExportExcel(headerId: any, data: any): Observable<any> {
    return this.commonService.post(`CalculateResult/ExportExcel?headerId=${headerId}`, data)
  }
  ExportWord(lstCustomerChecked: any, headerId : any): Observable<any> {
    return this.commonService.post(`CalculateResult/ExportWord?headerId=${headerId}`, lstCustomerChecked)
  }
  ExportWordTrinhKy(lstTrinhKyChecked: any, headerId : any): Observable<any> {
    return this.commonService.post(`CalculateResult/ExportWordTrinhKy?headerId=${headerId}`, lstTrinhKyChecked)
  }
  ExportPDF(lstCustomerChecked: any, headerId: any): Observable<any> {
    return this.commonService.post(`CalculateResult/ExportPDF?headerId=${headerId}`, lstCustomerChecked)
  }
  GetCustomer(): Observable<any> {
    return this.commonService.get(`CalculateResult/GetCustomer`)
  }
  SendMail(model: any): Observable<any> {
    return this.commonService.get(`CalculateResult/SendMail?headerId=${model}`)
  }
  SendSMS(model: any): Observable<any> {
    return this.commonService.get(`CalculateResult/SendSMS?headerId=${model}`)
  }
  Getmail(model: any): Observable<any> {
    return this.commonService.get(`CalculateResult/Getmail?headerId=${model}`)
  }
  GetSms(model: any): Observable<any> {
    return this.commonService.get(`CalculateResult/GetSms?headerId=${model}`)
  }
}
