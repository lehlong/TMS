import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { LocalFilter } from '../../models/master-data/local.model'
import { GlobalService } from '../../services/global.service'
import { LocalService } from '../../services/master-data/local.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder, FormControl } from '@angular/forms'
import {
  CALCULATE_RESULT_LIST_RIGHTS,
  CALCULATE_RESULT_RIGHT,
  IMPORT_BATCH,
} from '../../shared/constants'
import { NzMessageService } from 'ng-zorro-antd/message'
import { CalculateResultListFilter } from '../../models/calculate-result-list/calculate-result-list.model'
import { CalculateResultListService } from '../../services/calculate-result-list/calculate-result-list.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { Router } from '@angular/router'
import { SignerService } from '../../services/master-data/signer.service'
import { NzSelectModule } from 'ng-zorro-antd/select'
import { CustomerComponent } from '../../master-data/customer/customer.component'
import { CustomerService } from '../../services/master-data/customer.service'
@Component({
  selector: 'app-local',
  standalone: true,
  imports: [ShareModule,NzSelectModule],
  templateUrl: './calculate-result-list.component.html',
  styleUrl: './calculate-result-list.component.scss',
})
export class CalculateResultListComponent {
  validateForm: FormGroup = this.fb.group({
    code: [''],
    name: ['', [Validators.required]],
    status: [true],
    fDate: [''],
    isActive: [true, [Validators.required]],
  })

  nguoiKyControl = new FormControl({code:"",name:"",position:""});

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new CalculateResultListFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  isName: boolean = false
  IMPORT_BATCH = IMPORT_BATCH
  listspecialCustomer: any[] = []

  constructor(
    private _service: CalculateResultListService,
    private _customerService: CustomerService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
    private _goodsService: GoodsService,
    private _signerService: SignerService,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách đợt nhập',
        path: 'master-data/calculate-result-list',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }
  model: any = {
    header: {},
    hS1: [],
    hS2: [],
    fob: []
  }

  model_2: any = {
    header: {},
    hS1: [],
    hS2: [],
    fob: []
  }
  goodsResult: any[] = []
  signerResult: any[] = []
  selectedValue = {}
  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }

  ngOnInit(): void {
    this.search()
    this.getAllGoods()
    this.getAllSigner()
    console.log('calculate')
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
  onSortChange(name: string, value: any) {
    this.filter = {
      ...this.filter,
      SortColumn: name,
      IsDescending: value === 'descend',
    }
    this.search()
  }

  search() {
    this.isSubmit = false
    this._service.searchCalculateResultList(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some((local: any) => local.code === code)
  }

  submitForm(): void {
    if (this.model.header.name == '') {
      this.message.error(
        `Vui lòng nhập tên đợt nhập`,
      )
      // return
    }
    else if(this.nguoiKyControl.value?.name == ''){
      this.message.error(
        `Vui chọn người ký`,
      )
    }
    if (this.model.header.name != '') {
      this.model.header.signerCode = this.nguoiKyControl.value?.code || '';
      console.log(this.model)

      var m = {
        model: this.model,
      }
      
      this._service.createData(this.model).subscribe({
        next: (data) => {

          this.router.navigate([`/calculate-result/detail/${this.model.header.code}`])
          console.log(data)
        },
      })
    }
  }

  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new CalculateResultListFilter()
    this.search()
  }

  openCreate() {
    this.edit = false
    this.visible = true
    this._customerService.getSpecialCustomer().subscribe({
      next: (data) => {
        this.listspecialCustomer = data
      },
      error: (response) => {
        console.log(response)
      },
    })
    this._service.getObjectCreate().subscribe({
      next: (data) => {
        this.model = data
        this.model_2 = structuredClone(data)
        this.formatHSData()
        this.visible = true
      },
      error: (err) => {
        console.log(err)
      },
    })
  }
  formatHSData() {
    if (this.model_2.hS1 && Array.isArray(this.model_2.hS1)) {
      this.model_2.hS1.forEach((item:any) => {
        // Format các trường số cần format
        console.log("Cũ: " + item.heSoVcf);
        item.heSoVcf = this.formatNumber(item.heSoVcf);
        item.thueBvmt = this.formatNumber(item.thueBvmt);
      });
    }
  }
  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  deleteItem(code: string | number) {
    this._service.deleteCalculateResultList(code).subscribe({
      next: (data) => {
        this.search()
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  openEdit(data: any) {
    this.router.navigate([`/calculate-result/detail/${data.code}`])
  }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }

  getAllGoods() {
    this.isSubmit = false
    this._goodsService.getall().subscribe({
      next: (data) => {
        this.goodsResult = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  getAllSigner() {
    this.isSubmit = false
    this._signerService.getall().subscribe({
      next: (data) => {
        this.signerResult = data
        this.selectedValue = this.signerResult.find(item => item.code === "TongGiamDoc");
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  formatNumber(value: any): string {
    if (value == null || value === '') return '';
  
    const num = parseFloat(value.toString().replace(/,/g, ''));
    if (isNaN(num)) return '';
  
    // Format giữ 4 chữ số sau dấu phẩy (mày có thể chỉnh lại tuỳ)
    return num.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
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

  checkName(_name: string) {
    _name == '' ? (this.isName = true) : (this.isName = false)
  }
}
