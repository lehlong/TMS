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
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-calculate-result',
  standalone: true,
  imports: [ShareModule, NgxDocViewerModule, CommonModule],
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
  isVisiblePreview: boolean = false
  UrlOffice: string = ''
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

  model_2: any = {
    header: {},
    hS1: [],
    hS2: [],
    status: {
      code: '01',
      contents: '',
    },
  }
  checked = false
  lstCustomerChecked: any[] = []
  lstTrinhKyChecked: any[] = []
  signerResult: any[] = []
  selectedValue = {}
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
    this.getAllGoods()
    this.route.paramMap.subscribe({
      next: (params) => {
        const code = params.get('code')
        this.headerId = code
        if (this.accountGroups != 'G_NV_K') {
          this.changeTitle('DỮ LIỆU GỐC', 0)
        }
        else {
          this.changeTitle('PT', 1)
        }
        this._service.GetDataInput(this.headerId).subscribe({
          next: (data) => {
            console.log(this.goodsResult);

            data.hS2.sort((a: any, b: any) => {
              const indexA = this.goodsResult.findIndex(item => item.code === a.goodsCode);
              const indexB = this.goodsResult.findIndex(item => item.code === b.goodsCode);

              return indexA - indexB;
            });
            data.hS1.sort((a: any, b: any) => {
              const indexA = this.goodsResult.findIndex(item => item.code === a.goodsCode);
              const indexB = this.goodsResult.findIndex(item => item.code === b.goodsCode);

              return indexA - indexB;
            });
            console.log(data);

            this.model = data
            this.model_2 = structuredClone(data)
            this.formatHSData()
          },
        })
      },
    })

  }
  formatHSData() {
    if (this.model_2.hS1 && Array.isArray(this.model_2.hS1)) {
      this.model_2.hS1.forEach((item: any) => {
        // Format các trường số cần format
        console.log("Cũ: " + item.heSoVcf);
        item.heSoVcf = this.formatNumber(item.heSoVcf);
        item.thueBvmt = this.formatNumber(item.thueBvmt);
        item.l15ChuaVatBvmt = this.formatNumber(item.l15ChuaVatBvmt);
        item.l15ChuaVatBvmtNbl = this.formatNumber(item.l15ChuaVatBvmtNbl);
        item.giamGiaFob = this.formatNumber(item.giamGiaFob);
        item.laiGopDieuTiet = this.formatNumber(item.laiGopDieuTiet);
      });
    }

    if (this.model_2.hS2 && Array.isArray(this.model_2.hS2)) {
      this.model_2.hS2.forEach((item: any) => {
        item.gblcsV1 = this.formatNumber(item.gblcsV1);
        item.gblV2 = this.formatNumber(item.gblV2);
        item.v2_V1 = this.formatNumber(item.v2_V1);
        item.mtsV1 = this.formatNumber(item.mtsV1);
        item.gny = this.formatNumber(item.gny);
        item.clgblv = this.formatNumber(item.clgblv);


      });
    }
  }

  formatNumber(value: any): string {
    if (value == null || value === '') return '';

    const num = parseFloat(value.toString().replace(/,/g, ''));
    if (isNaN(num)) return '';

    // Format giữ 4 chữ số sau dấu phẩy (mày có thể chỉnh lại tuỳ)
    return num.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
  }

  getRight() {
    const rights = localStorage.getItem('userRights');
    this.rightList = rights ? JSON.parse(rights) : [];

    const accountGroups = localStorage.getItem('UserInfo');
    this.accountGroups = accountGroups ? JSON.parse(accountGroups).accountGroups[0].name : [];
  }


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
    this.searchTerm = ''
    this.searchInput = ''
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

  onInputNumberFormat(data: any, field: string, isHs2: boolean) {
    let value = data[field];

    // 1. Bỏ ký tự không hợp lệ (chỉ giữ số, '-', '.')
    value = value.replace(/[^0-9\-.]/g, '');

    // 2. Đảm bảo chỉ có 1 dấu '-' và nó đứng đầu
    const minusMatches = value.match(/-/g);
    if (minusMatches && minusMatches.length > 1) {
      value = value.replace(/-/g, ''); // Xoá hết
      value = '-' + value; // Thêm 1 dấu '-' đầu tiên
    } else if (minusMatches && !value.startsWith('-')) {
      value = value.replace(/-/g, '');
      value = '-' + value;
    }

    // 3. Xử lý dấu '.': chỉ cho sau '0' hoặc '-0' và duy nhất
    const dotIndex = value.indexOf('.');
    if (dotIndex !== -1) {
      const beforeDot = value.substring(0, dotIndex);
      const afterDot = value.substring(dotIndex + 1).replace(/\./g, '');

      if (beforeDot === '0' || beforeDot === '-0') {
        value = beforeDot + '.' + afterDot;
      } else {
        // Loại bỏ dấu '.' nếu không đúng điều kiện
        value = beforeDot + afterDot;
      }
    }

    // 4. Format phần nguyên với dấu ','
    const parts = value.split('.');
    let integerPart = parts[0].replace(/[^0-9\-]/g, ''); // giữ dấu '-'
    integerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    // 5. Ghép lại
    let formattedValue = integerPart;
    if (parts[1]) {
      formattedValue += '.' + parts[1];
    }

    // 6. Cập nhật lại giá trị hiển thị
    data[field] = formattedValue;

    // 7. Parse về số
    const rawNumber = formattedValue.replace(/,/g, '');
    const numberValue = parseFloat(rawNumber);
    const finalNumber = isNaN(numberValue) ? 0 : numberValue;

    // 8. Update vào model chuẩn
    if (isHs2) {
      const index = this.model.hS2.findIndex((x: any) => x.goodsCode === data.goodsCode);
      if (index !== -1) {
        this.model.hS2[index][field] = finalNumber;
      }
    } else {
      const index = this.model.hS1.findIndex((x: any) => x.goodsCode === data.goodsCode);
      if (index !== -1) {
        this.model.hS1[index][field] = finalNumber;
      }
    }
  }

  onKeyDownNumberOnly(event: KeyboardEvent) {
    const allowedKeys = [
      'Backspace', 'ArrowLeft', 'ArrowRight', 'Delete', 'Tab', '-', '.', // Thêm "-" và "."
    ];

    if (
      (event.key >= '0' && event.key <= '9') || allowedKeys.includes(event.key)
    ) {
      return; // Cho phép số, -, .
    } else {
      event.preventDefault(); // Chặn ký tự khác
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
          item.pathView = environment.apiUrl + item.path
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
    const index = this.model_2.hS2.indexOf(row)

    this.model.hS2[index].v2_V1 = this.model.hS2[index].gblV2 - this.model.hS2[index].gblcsV1
    this.model.hS2[index].gny = this.model.hS2[index].gblcsV1 + this.model.hS2[index].mtsV1
    this.model.hS2[index].clgblv = this.model.hS2[index].gblV2 - this.model.hS2[index].gny

    this.model_2.hS2[index].v2_V1 = this.formatNumber(this.model.hS2[index].v2_V1)
    this.model_2.hS2[index].gny = this.formatNumber(this.model.hS2[index].gny)
    this.model_2.hS2[index].clgblv = this.formatNumber(this.model.hS2[index].clgblv)
  }
  updateDataInput() {
    if (this.model.header.name != '') {
      console.log(this.model)
      console.log(this.model)
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
  onCurrentPageDataChange($event: any): void { }

  fullScreen() {
    this.isZoom = true
    document.documentElement.requestFullscreen()
  }
  closeFullScreen() {
    this.isZoom = false
    document
      .exitFullscreen()
      .then(() => { })
      .catch(() => { })
  }

  cancelSendSMS() { }
  cancelSendEmail() { }

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
    console.log(url)
    window.open(url, '_blank')
  }
  Preview(url: string) {

    this.UrlOffice = url
    console.log(this.UrlOffice)
    this.isVisiblePreview = true

  }
  cancelPreview() {
    this.isVisiblePreview = !this.isVisiblePreview
  }
  checkName(_name: string) {
    _name == '' ? (this.isName = true) : (this.isName = false)
  }
}
