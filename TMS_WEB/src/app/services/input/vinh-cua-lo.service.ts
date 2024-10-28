import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class VinhCuaLoService {
  constructor(private commonService: CommonService) {}

  searchVinhCuaLo(params: any): Observable<any> {
    return this.commonService.get('VinhCuaLo/Search', params)
  }

  getall(): Observable<any> {
    return this.commonService.get('VinhCuaLo/GetAll')
  }

  createVinhCuaLo(params: any): Observable<any> {
    return this.commonService.post('VinhCuaLo/Insert', params)
  }

  updateVinhCuaLo(params: any): Observable<any> {
    return this.commonService.put('VinhCuaLo/Update', params)
  }

  deleteVinhCuaLo(id: string): Observable<any> {
    return this.commonService.delete(`VinhCuaLo/Delete/${id}`)
  }
  exportExcelVinhCuaLo(params: any): Observable<any> {
    return this.commonService.downloadFile('VinhCuaLo/Export', params)
  }
}
