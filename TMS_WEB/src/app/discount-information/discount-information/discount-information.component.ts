import { Component } from '@angular/core';
import { GlobalService } from '../../services/global.service';
import { ShareModule } from '../../shared/share-module';
import { ActivatedRoute } from '@angular/router';
import { GoodsService } from '../../services/master-data/goods.service';
import { DiscountInformationService } from '../../services/discount-information/discount-information.service';
import { DiscountInformationListService } from '../../services/discount-information/discount-information-list.service';
import { environment } from '../../../environments/environment.prod';

@Component({
  selector: 'app-discount-information',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './discount-information.component.html',
  styleUrl: './discount-information.component.scss'
})
export class DiscountInformationComponent {
  constructor(
    private _service: DiscountInformationService,
    private _discountInformationList: DiscountInformationListService,
    private globalService: GlobalService,
    private route: ActivatedRoute,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Kết quả tính toán đầu ra',
        path: 'calculate-result',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }


  loading: boolean = false
  edit: boolean = false
  visible: boolean = false
  code: any = ''
  title: any = 'Phân tích chiết khấu'
  name: any = ''
  headerId: any = ''
  fDate: any = ''
  data: any = {
    lstDIL: [{}],
    lstGoods : [],
    lstCompetitor : [],
    discount: []
  }
  model: any = {
    goodss: [{
      code: '',
      hs: []
    }],
    header: {
      name: '',
      fData: ''
    }
  }

  ngOnInit() {
    this.route.paramMap.subscribe({
      next: (params) => {
        this.code = params.get('code')
        this.getAll()

      },
    })
    // this.changeTitle()
    console.log(this.name);


    console.log(this.title);
  }

  getAll() {
    this._service.getAll(this.code).subscribe({
      next: (data) => {
        this.data = data
        console.log(this.data);
        this.name = this.data.lstDIL[0].name
        this.fDate = new Date(this.data.lstDIL[0].fDate).toLocaleDateString()
        this.changeTitle(this.name, this.fDate)
      },
      error: (response) => {
        console.log(response)
      },
    })
  }

  submitForm(): void {
    console.log(this.model)
    var m = {
      model: this.model
    }
    this._discountInformationList.createData(this.model).subscribe({
      next: (data) => {
        console.log(data)
      }
    })
  }
  changeTitle(name: string, fDate: string){
    this.title = 'Phân tích chiết khấu ngày ' + fDate
    console.log(name + fDate);

  }

  getDataHeader(){
    this._discountInformationList.getObjectCreate(this.code).subscribe({
      next: (data) => {
        this.visible = true
        this.model = data
      },
    })
  }
  reCalculate(){
    this.getAll()
  }
  showHistoryExport(){

  }
  exportExcel(){
    this._service.ExportExcel(this.code).subscribe({
      next: (data) => {
        var a = document.createElement('a')
        a.href = environment.apiUrl + data
        a.target = '_blank'
        a.click()
        a.remove()
      },
    })
  }


  close() {
    this.visible = false
  }
}
