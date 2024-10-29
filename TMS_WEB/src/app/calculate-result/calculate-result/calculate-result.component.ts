import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { CalculateResultService } from '../../services/calculate-result/calculate-result.service'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute } from '@angular/router'
import { GoodsService } from '../../services/master-data/goods.service'
import { CALCULATE_RESULT_RIGHT } from '../../shared/constants/access-right.constants'

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
    private route: ActivatedRoute,
    private _goodsService: GoodsService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Kết quả tính toán đầu ra',
        path: 'calculate-result',
      },
    ])
  }
  title: string = 'DỮ LIỆU GỐC'
  CALCULATE_RESULT_RIGHT = CALCULATE_RESULT_RIGHT
  isVisibleHistory: boolean = false
  visibleDrawer: boolean = false
  isVisibleStatus: boolean = false
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
    pL1: [],
    pL2: [],
    pL3: [],
    pL4: [],
    fob: [],
    vK11PT: [],
    vK11DB: [],
    vK11FOB: [],
    vK11TNPP: [],
  }

  statusModel = {
    title: '',
    des: '',
    value: '',
  }

  headerId: any = ''

  model: any = {
    header: {},
    hS1: [],
    hS2: [],
    status: {
      code: '',
      contents: '',
    },
  }
  lstHistory: any[] = []
  goodsResult: any[] = []

  ngOnInit() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const code = params.get('code')
        this.headerId = code
        this.GetData(code);
        this._service.GetDataInput(this.headerId).subscribe({
          next: (data) => {
            this.model = data
          },
        })
      },
    })
    this.getAllGoods()
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  GetData(code: any) {
    this._service.GetResult(code).subscribe({
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
  changeStatus(value: string, status: string) {
    switch (value) {
      case '01':
        this.statusModel.title = 'TRÌNH DUYỆT'
        this.statusModel.des = 'Bạn có muốn Trình duyệt dữ liệu này?'
        break
      case '02':
        this.statusModel.title = 'YÊU CẦU CHỈNH SỬA'
        this.statusModel.des = 'Bạn có muốn Yêu cầu chỉnh sửa lại dữ liệu này?'
        break
      case '03':
        this.statusModel.title = 'PHÊ DUYỆT'
        this.statusModel.des = 'Bạn có muốn Phê duyệt dữ liệu này?'
        break
      case '04':
        this.statusModel.title = 'TỪ CHỐI'
        this.statusModel.des = 'Bạn có muốn Từ chối dữ liệu này?'
        break
    }
    this.model.status.code = status
    this.isVisibleStatus = true
  }
  showHistoryAction() {
    this._service.GetHistoryAction(this.headerId).subscribe({
      next: (data) => {
        this.lstHistory = data
        console.log(data)
        this.isVisibleHistory = true
      },
      error: (err) => {
        console.log(err)
      },
    })
  }
  handleOk(): void {
    this.isVisibleHistory = false
    this.isVisibleStatus = false
  }
  handleOkStatus(): void {
    this.model.status.contents = this.statusModel.value;
    this.updateDataInput();
    this.isVisibleStatus = false
  }

  handleCancel(): void {
    this.isVisibleHistory = false
    this.isVisibleStatus = false
  }
  reCalculate() {
    this.GetData(this.headerId)
  }
  closeDrawer() {
    this.visibleDrawer = false
  }
  getDataHeader() {
    this._service.GetDataInput(this.headerId).subscribe({
      next: (data) => {
        this.model = data
        this.visibleDrawer = true
      },
    })
  }
  getAllGoods() {
    this._goodsService.getall().subscribe({
      next: (data) => {
        this.goodsResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  onKeyUpCalculate(row: any) {
    row.v2_V1 = row.gblV2 - row.gblcsV1
    row.gny = row.gblcsV1 + row.mtsV1
    row.clgblv = row.gblV2 - row.gny
  }
  updateDataInput() {
    this._service.UpdateDataInput(this.model).subscribe({
      next: (data) => {
        console.log(data)
        window.location.reload();
      },
      error: (err) => {
        console.log(err)
      },
    })
  }
}
