import { Injectable } from '@angular/core'
import { CommonService } from '../common.service'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CalculateResultService {
  constructor(private commonService: CommonService) {}

  GetResult(model : any): Observable<any> {
    return this.commonService.post('CalculateResult/GetCalculateResult', model)
  }
}
