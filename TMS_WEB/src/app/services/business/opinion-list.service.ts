import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class OpinionListService {
  constructor(private commonService: CommonService) { }

  GetOplTree() {
    return this.commonService.get('OpinionList/GetOpinionListTree')
  }
  GetOplTreeWithMgCode(MgCode: string): Observable<any> {
    return this.commonService.get(
      `OpinionList/GetOpinionListTreeWithMgCode/${MgCode}`,
    )
  }

  GetOplTreeWithMgCodeAndOrg(OrgCode: string, MgCode: string): Observable<any> {
    return this.commonService.get(
      `OpinionList/GetOplTreeWithMgCodeAndOrg/${OrgCode}/${MgCode}`,
    )
  }

  GetOpinionDetail(OrgCode: string, MgCode: string, OpinionCode: string): Observable<any> {
    return this.commonService.get(
      `OpinionList/GetOpinionDetail/${MgCode}/${OrgCode}/${OpinionCode}`,
    )
  }

  GetOpinionListTreeWithTimeYearAndAuditPeriod(
    TimeYear: string,
    AuditPeriod: string,
  ): Observable<any> {
    return this.commonService.get(
      'OpinionList/GetOpinionListTreeWithTimeYearAndAuditPeriod',
      { TimeYear, AuditPeriod },
    )
  }
  GetOrgInOpinion(OpinionCode: string): Observable<any> {
    return this.commonService.get(`OpinionList/GetOrgInOpinion/${OpinionCode}`)
  }

  Update(data: any) {
    return this.commonService.put('OpinionList/Update', data)
  }
  UpdateOpinionDetail(data: any) {
    return this.commonService.put('OpinionList/UpdateOpinionDetail', data)
  }
  Insert(data: any) {
    return this.commonService.post('OpinionList/Insert', data)
  }

  UpdateOrderTree(dataTree: any) {
    return this.commonService.put('OpinionList/Update-Order', dataTree)
  }

  Delete(code: string): Observable<any> {
    return this.commonService.delete(`OpinionList/Delete/${code}`)
  }
  uploadExcel(
    file: File,
    mgCode: string,
  ): Observable<any> {
    const formData = new FormData()
    formData.append('file', file)
    return this.commonService.put(
      `OpinionList/UploadExcel?mgCode=${mgCode}`,
      formData,
    )
  }
}
