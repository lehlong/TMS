import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {CommonService} from '../common.service';

@Injectable({
  providedIn: 'root',
})
export class CalculateResultListService {
  constructor(private commonService: CommonService) {}

  searchCalculateResultList(params: any): Observable<any> {
    return this.commonService.get('CalculateResultList/Search', params);
  }

  getall(): Observable<any> {
    return this.commonService.get('CalculateResultList/GetAll');
  }

  getObjectCreate(): Observable<any> {
    return this.commonService.get('CalculateResultList/GetObjectCreate');
  }

  createData(params: any): Observable<any> {
    return this.commonService.post('CalculateResultList/Insert', params);
  }

  updateCalculateResultList(params: any): Observable<any> {
    return this.commonService.put('CalculateResultList/Update', params);
  }

  exportExcelCalculateResultList(params: any): Observable<any> {
    return this.commonService.downloadFile('CalculateResultList/Export', params);
  }

  deleteCalculateResultList(id: string | number): Observable<any> {
    return this.commonService.delete(`CalculateResultList/Delete/${id}`);
  }
}
