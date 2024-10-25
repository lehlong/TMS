import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { CalculateResultService } from '../../services/calculate-result/calculate-result.service'
import { GlobalService } from '../../services/global.service'

@Component({
  selector: 'app-calculate-result',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './calculate-result.component.html',
  styleUrl: './calculate-result.component.scss',
})
export class CalculateResultComponent {
  constructor(
    private _service: CalculateResultService,
    private globalService: GlobalService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Kết quả tính toán đầu ra',
        path: 'calculate-result',
      },
    ])
  }
  title: string = 'DỮ LIỆU GỐC'
  date = new Date()
  model = {
    fDate: '',
  }
  data: any = {
    lstGoods: [],
    dlg: {
      dlg_1: [],
      dlg_2: [],
      dlg_3: [],
      dlg_4: [],
      dlg_5: [],
      dlg_6: [],
    },
    pt: [],
    db: [],
    pT09: [],
    pL2: [],
    pL4: [],
    fob: [],
  }

  ngOnInit() {
    this.GetData(this.model)
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }
  onChangeDate(result: any): void {
    console.log('onChange: ', result)
  }
  GetData(model: any) {
    var _fd = new Date(this.date)
    model.fDate = _fd.toLocaleString()
    this._service.GetResult(this.model).subscribe({
      next: (data) => {
        this.data = data
        console.log(data)
      },
      error: (e) => {
        console.log(e)
      },
    })
  }
  changeTitle(value: string) {
    this.title = value
  }
}
