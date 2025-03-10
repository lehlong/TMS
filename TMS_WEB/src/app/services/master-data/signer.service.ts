import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class SignerService {
  constructor(private commonService: CommonService) {}

  searchSigners(params: any): Observable<any> {
    return this.commonService.get('Signer/Search', params)
  }

  getall(): Observable<any> {
    return this.commonService.get('Signer/GetAll')
  }

  createSigner(params: any): Observable<any> {
    return this.commonService.post('Signer/Insert', params)
  }

  updateSigner(params: any): Observable<any> {
    return this.commonService.put('Signer/Update', params)
  }
}
