import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class GiaGiaoTapDoanService {
  constructor(private commonService: CommonService) {}

  searchGiaGiaoTapDoan(params: any): Observable<any> {
    return this.commonService.get('GiaGiaoTapDoan/Search', params)
  }

  getall(): Observable<any> {
    return this.commonService.get('GiaGiaoTapDoan/GetAll')
  }

  createGiaGiaoTapDoan(params: any): Observable<any> {
    return this.commonService.post('GiaGiaoTapDoan/Insert', params)
  }

  updateGiaGiaoTapDoan(params: any): Observable<any> {
    return this.commonService.put('GiaGiaoTapDoan/Update', params)
  }

  deleteGiaGiaoTapDoan(code: string | number): Observable<any> {
    return this.commonService.delete(`GiaGiaoTapDoan/Delete/${code}`)
  }
  exportExcelGiaGiaoTapDoan(params: any): Observable<any> {
    return this.commonService.downloadFile('GiaGiaoTapDoan/Export', params)
  }
}
