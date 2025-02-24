import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { CalculateResultService } from '../../services/calculate-result/calculate-result.service'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute } from '@angular/router'
import { GoodsService } from '../../services/master-data/goods.service'
import { CALCULATE_RESULT_RIGHT } from '../../shared/constants/access-right.constants'
import { environment } from '../../../environments/environment.prod'
import { NzMessageService } from 'ng-zorro-antd/message'

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
    private message: NzMessageService,
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
  isVisibleExport: boolean = false
  isVisibleCustomer: boolean = false
  isVisibleCustomerPDF: boolean = false
  isName: boolean = false

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
    vK11BB: [],
    bbdo: [],
    bbfo: [],
    summary: [],
  }

  statusModel = {
    title: '',
    des: '',
    value: '',
  }

  headerId: any = ''
  isZoom = false

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
  lstHistoryFile: any[] = []
  goodsResult: any[] = []
  lstCustomer: any[] = []
  ngOnInit() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const code = params.get('code')
        this.headerId = code
        this.GetData(code)
        this._service.GetDataInput(this.headerId).subscribe({
          next: (data) => {
            this.model = data
          },
        })
      },
    })
    this.getAllGoods()
  }

  checked = false
  lstCustomerChecked: any[] = []

  updateCheckedSet(code: any, checked: boolean): void {
    if (checked) {
      this.lstCustomerChecked.push(code)
    } else {
      this.lstCustomerChecked = this.lstCustomerChecked.filter((x) => x != code)
    }
  }
  onItemChecked(code: number, checked: boolean): void {
    this.updateCheckedSet(code, checked)
  }
  onAllChecked(value: boolean): void {
    this.lstCustomerChecked = []
    if (value) {
      this.lstCustomer.forEach((i) => {
        this.lstCustomerChecked.push(i.code)
      })
    } else {
      this.lstCustomerChecked = []
    }
  }

  confirmExportWord() {
    if (this.lstCustomerChecked.length == 0) {
      this.message.create(
        'warning',
        'Vui lòng chọn khách hàng cần xuất ra file',
      )
      return
    } else {
      this._service.ExportWord(this.lstCustomerChecked, this.headerId).subscribe({
        next: (data) => {
          this.isVisibleCustomer = false
          this.lstCustomerChecked = []
          var a = document.createElement('a')
          a.href = environment.apiUrl + data
          a.target = '_blank'
          a.click()
          a.remove()
        },
        error: (err) => {
          console.log(err)
        },
      })
    }
  }

  confirmExportPDF() {
    if (this.lstCustomerChecked.length == 0) {
      this.message.create(
        'warning',
        'Vui lòng chọn khách hàng cần xuất ra file',
      )
      return
    } else {
      this._service.ExportPDF(this.lstCustomerChecked, this.headerId).subscribe({
        next: (data) => {
          this.isVisibleCustomer = false
          this.lstCustomerChecked = []
          var a = document.createElement('a')
          a.href = environment.apiUrl + data
          a.target = '_blank'
          a.click()
          a.remove()
        },
        error: (err) => {
          console.log(err)
        },
      })
    }
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
  showHistoryExport() {
    this._service.GetHistoryFile(this.headerId).subscribe({
      next: (data) => {
        this.lstHistoryFile = data
        this.isVisibleExport = true
        this.lstHistoryFile.forEach((item) => {
          item.pathDownload = environment.apiUrl + item.path
          item.pathView = `https://view.officeapps.live.com/op/embed.aspx?src=${environment.apiUrl}${item.path}`
        })
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
    this.model.status.contents = this.statusModel.value
    this.updateDataInput()
    this.isVisibleStatus = false
  }

  handleCancel(): void {
    this.isVisibleHistory = false
    this.isVisibleStatus = false
    this.isVisibleExport = false
    this.isVisibleCustomer = false
    this.isVisibleCustomerPDF = false
    this.lstCustomerChecked = [];
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
    if (this.model.header.name != ''){

      this._service.UpdateDataInput(this.model).subscribe({
        next: (data) => {
          console.log(data)
          window.location.reload()
        },
        error: (err) => {
          console.log(err)
        },
      })
    }
  }

  exportExcel() {
    this._service.ExportExcel(this.headerId).subscribe({
      next: (data) => {
        var a = document.createElement('a')
        a.href = environment.apiUrl + data
        a.target = '_blank'
        a.click()
        a.remove()
      },
    })
  }
  exportWord() {
    this._service.GetCustomer().subscribe({
      next: (data) => {
        this.lstCustomer = data
        this.isVisibleCustomer = true
      },
    })
  }
  exportPDF() {
    this._service.GetCustomer().subscribe({
      next: (data) => {
        this.lstCustomer = data
        this.isVisibleCustomerPDF = true
      },
    })
  }
  onCurrentPageDataChange($event: any): void {}

  fullScreen(){
    this.isZoom = true
    document.documentElement.requestFullscreen()
  }
  closeFullScreen(){
    this.isZoom = false
    document.exitFullscreen()
      .then(() => {
    })
      .catch(() => {
    })
  }

  cancelSendSMS() {}

  confirmSendSMS() {}

  openNewTab(url : string){
    window.open(url, '_blank')
  }


  checkName(_name: string){
    _name == '' ? this.isName = true : this.isName = false
  }
}
