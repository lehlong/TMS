import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class HeSoMatHangService {
  constructor(private commonService: CommonService) {}

  searchHeSoMatHang(params: any): Observable<any> {
    return this.commonService.get('HeSoMatHang/Search', params)
  }

  getall(): Observable<any> {
    return this.commonService.get('HeSoMatHang/GetAll')
  }

  createHeSoMatHang(params: any): Observable<any> {
    return this.commonService.post('HeSoMatHang/Insert', params)
  }

  updateHeSoMatHang(params: any): Observable<any> {
    return this.commonService.put('HeSoMatHang/Update', params)
  }

  deleteHeSoMatHang(id: string): Observable<any> {
    return this.commonService.delete(`HeSoMatHang/Delete/${id}`)
  }
  exportExcelHeSoMatHang(params: any): Observable<any> {
    return this.commonService.downloadFile('HeSoMatHang/Export', params)
  }
}
