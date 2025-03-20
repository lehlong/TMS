import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { CalculateResultService } from '../../services/calculate-result/calculate-result.service'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute } from '@angular/router'
import { GoodsService } from '../../services/master-data/goods.service'
import {
  CALCULATE_RESULT_RIGHT,
  IMPORT_BATCH,
} from '../../shared/constants/access-right.constants'
import { environment } from '../../../environments/environment.prod'
import { NzMessageService } from 'ng-zorro-antd/message'
import { SignerService } from '../../services/master-data/signer.service'
import { FormControl } from '@angular/forms'

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
    private _signerService: SignerService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Kết quả tính toán đầu ra',
        path: 'calculate-result',
      },
    ])
  }
  searchInput = ''
  searchTerm: string = ''
  nguoiKyControl = new FormControl({ name: '', position: '' })
  rightList: any = []
  title: string = ''
  IMPORT_BATCH = IMPORT_BATCH
  isVisibleHistory: boolean = false
  visibleDrawer: boolean = false
  isVisibleStatus: boolean = false
  isVisibleEmail: boolean = false
  isVisibleSms: boolean = false
  isVisibleExport: boolean = false
  isVisibleCustomer: boolean = false
  isVisibleLstTrinhKy: boolean = false
  isVisibleCustomerPDF: boolean = false
  isName: boolean = false
  accountGroups: any = {}
  data: any = {
    nameOld: '',
    lstGoods: [],
    dlg: {
      dlg_1: [],
      dlg_2: [],
      dlg_3: [],
      dlg_4: [],
      dlg_5: [],
      dlg_6: [],
      dlg_7: [],
      dlg_8: [],
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
      code: '01',
      contents: '',
    },
  }

  lstHistory: any[] = []
  lstHistoryFile: any[] = []
  lstSMS: any[] = []
  lstEmail: any[] = []
  goodsResult: any[] = []
  lstCustomer: any[] = []
  lstTrinhKy: any[] = [
    {
      code: 'CongDienKKGiaBanLe',
      name: 'Công Điện Kiểm Kê Giá Bán Lẻ',
      status: true,
    },
    { code: 'MucGiamGiaNQTM', name: 'Mức Giảm Giá NQTM', status: false },
    { code: 'QDGBanBuon', name: 'Quyết Định Giá bán Buôn', status: false },
    { code: 'QDGBanLe', name: 'Quyết Định Giá Bán lẻ', status: true },
    { code: 'QDGCtyPTS', name: 'Quyết Định Công Ty PTS', status: false },
    { code: 'QDGNoiDung', name: 'Quyết Giá Nội Dung', status: false },
    { code: 'ToTrinh', name: 'Tờ Trình', status: false },
    { code: 'KeKhaiGia', name: 'Kê Khai Giá', status: true },
    { code: 'KeKhaiGiaChiTiet', name: 'Kê Khai Giá Chi Tiết', status: true },
  ]
  ngOnInit() {
    this.getRight()
    this.route.paramMap.subscribe({
      next: (params) => {
        const code = params.get('code')
        this.headerId = code
        if(this.accountGroups != 'G_NV_K'){
          this.changeTitle('DỮ LIỆU GỐC',0)
        }
        else{
          this.changeTitle('PT',1)
        }
        this._service.GetDataInput(this.headerId).subscribe({
          next: (data) => {
            this.model = data
          },
        })
      },
    })
    this.getAllGoods()
    // if (this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_DLG)) {
    //   this.changeTitle('DỮ LIỆU GỐC')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PT)
    // ) {
    //   this.changeTitle('PT')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_DB)
    // ) {
    //   this.changeTitle('DB')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_FOB)
    // ) {
    //   this.changeTitle('FOB')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PT09)
    // ) {
    //   this.changeTitle('PT09')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_BB_DO)
    // ) {
    //   this.changeTitle('BB DO')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_BB_FO)
    // ) {
    //   this.changeTitle('BB FO')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PL1)
    // ) {
    //   this.changeTitle('PL1')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PL2)
    // ) {
    //   this.changeTitle('PL2')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PL3)
    // ) {
    //   this.changeTitle('PL3')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PL4)
    // ) {
    //   this.changeTitle('PL4')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_VK11_PT)
    // ) {
    //   this.changeTitle('VK11 PT')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_VK11_DB)
    // ) {
    //   this.changeTitle('VK11 ĐB')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_VK11_FOB)
    // ) {
    //   this.changeTitle('VK11-FOB')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_VK11_TNPP)
    // ) {
    //   this.changeTitle('VK11-TNPP')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_PTS)
    // ) {
    //   this.changeTitle('PTS')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_VK11_BB)
    // ) {
    //   this.changeTitle('VK11 BB')
    // } else if (
    //   this.rightList.includes(IMPORT_BATCH.WATCH_IMPORT_BATCH_LIST_TONG_HOP)
    // ) {
    //   this.changeTitle('TỔNG HỢP')
    // } else {
    //   this.changeTitle('')
    // }
  }

  getRight() {
    const rights = localStorage.getItem('userRights');
    this.rightList = rights ? JSON.parse(rights) : [];

    const accountGroups = localStorage.getItem('UserInfo');
    this.accountGroups = accountGroups ? JSON.parse(accountGroups).accountGroups[0].name : [];
  }

  checked = false
  lstCustomerChecked: any[] = []
  lstTrinhKyChecked: any[] = []
  signerResult: any[] = []
  selectedValue = {}

  updateTrinhKyCheckedSet(code: any, checked: boolean): void {
    if (checked) {
      this.lstTrinhKyChecked.push(code)
    } else {
      this.lstTrinhKyChecked = this.lstTrinhKyChecked.filter((x) => x != code)
    }
  }

  onItemTrinhKyChecked(code: String, checked: boolean): void {
    this.updateTrinhKyCheckedSet(code, checked)
    console.log(this.lstTrinhKyChecked)
  }

  updateCheckedSet(code: any, checked: boolean): void {
    if (checked) {
      this.lstCustomerChecked.push(code)
    } else {
      this.lstCustomerChecked = this.lstCustomerChecked.filter((x) => x != code)
    }
  }

  onItemChecked(code: String, checked: boolean): void {
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

  onAllCheckedLstTrinhKy(value: boolean): void {
    this.lstTrinhKyChecked = []
    if (value) {
      this.lstTrinhKy.forEach((i) => {
        this.lstTrinhKyChecked.push(i.code)
      })
    } else {
      this.lstTrinhKyChecked = []
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
      this._service
        .ExportWord(this.lstCustomerChecked, this.headerId)
        .subscribe({
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
  search() {
    this.searchTerm = this.searchInput;
  }
  reset() {
    this.searchTerm =''
    this.searchInput=''
  }
  getAllSigner() {
    this._signerService.getall().subscribe({
      next: (data) => {
        this.signerResult = data
        // console.log(data)

        // this.selectedValue = this.signerResult.find(item => item.code === this.model.header.signerCode);
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  confirmExportWordTrinhKy() {
    if (this.lstTrinhKyChecked.length == 0) {
      this.message.create('warning', 'Vui lòng chọn trình ky xuất ra file')
      return
    } else {
      this._service
        .ExportWordTrinhKy(this.lstTrinhKyChecked, this.headerId)
        .subscribe({
          next: (data) => {
            this.isVisibleCustomer = false
            this.lstTrinhKyChecked = []
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
      this.lstTrinhKyChecked = []
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
      this._service
        .ExportPDF(this.lstCustomerChecked, this.headerId)
        .subscribe({
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

  GetData(code: any, tab: any) {
    if (code == null || code == null) {
      code = this.headerId;
    }
    this._service.GetResult(code, tab).subscribe({
      next: (data) => {
        this.data = data
        // console.log(data)
      },
      error: (e) => {
        console.log(e)
      },
    })
  }

  changeTitle(value: string, tab: any) {
    this.reset()
    this.title = value
    this.GetData(this.headerId, tab);
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
      case '05':
        this.statusModel.title = 'HỦY TRÌNH DUYỆT'
        this.statusModel.des = 'Bạn có muốn Hủy trình duyệt dữ liệu này?'
        break
      case '06':
        this.statusModel.title = 'HỦY PHÊ DUYỆT'
        this.statusModel.des = 'Bạn có muốn Hủy phê duyệt dữ liệu này?'
        break
    }
    this.model.status.code = status
    this.isVisibleStatus = true
  }
  showHistoryAction() {
    this._service.GetHistoryAction(this.headerId).subscribe({
      next: (data) => {
        this.lstHistory = data
        // console.log(data)
        this.isVisibleHistory = true
      },
      error: (err) => {
        console.log(err)
      },
    })
  }
  showEmailAction() {
    this._service.Getmail(this.headerId).subscribe({
      next: (data) => {
        this.lstEmail = data
        console.log(data)
        this.isVisibleEmail = true
      },
      error: (err) => {
        console.log(err)
      },
    })
  }
  removeHtmlTags(html: string): string {
    if (!html) return ''; 
    return html.replace(/<\/?[^>]+(>|$)/g, ""); 
  }
  showSMSAction() {
  
    this._service.GetSms(this.headerId).subscribe({
      next: (data) => {
        this.lstSMS = data
        console.log(data)
        this.isVisibleSms = true
        
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
    this.lstCustomerChecked = []
    this.isVisibleEmail = false
    this.isVisibleSms = false
  }
  reCalculate() {
    this.GetData(this.headerId, 0)
  }
  closeDrawer() {
    this.visibleDrawer = false
  }
  getDataHeader() {
    this.getAllSigner()
    this.visibleDrawer = true
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
    if (this.model.header.name != '') {
      // console.log(this.model)
      this._service.UpdateDataInput(this.model).subscribe({
        next: (data) => {
          window.location.reload()
        },
        error: (err) => {
          console.log(err)
        },
      })
    }
  }

  exportExcel() {
    this._service.ExportExcel(this.data, this.headerId).subscribe({
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

  exportWordTrinhKy() {
    this.isVisibleLstTrinhKy = !this.isVisibleLstTrinhKy
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

  fullScreen() {
    this.isZoom = true
    document.documentElement.requestFullscreen()
  }
  closeFullScreen() {
    this.isZoom = false
    document
      .exitFullscreen()
      .then(() => {})
      .catch(() => {})
  }

  cancelSendSMS() {}
  cancelSendEmail() {}

  confirmSendSMS() {
    console.log("err")
    this._service.SendSMS(this.headerId).subscribe({
    
      next: (data) => {
        this.message.create('success', 'Gửi mail thành công')
      },
      error: (err) => {
        console.log(err)
      },
    })
  }

  confirmSendsMail() {
    console.log("err")
    this._service.SendMail(this.headerId).subscribe({
    
      next: (data) => {
        this.message.create('success', 'Gửi mail thành công')
      },
      error: (err) => {
        console.log(err)
      },
    })
  }

  openNewTab(url: string) {
    window.open(url, '_blank')
  }

  checkName(_name: string) {
    _name == '' ? (this.isName = true) : (this.isName = false)
  }
}
