import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { CalculateResultService } from '../../services/calculate-result/calculate-result.service'
import { GlobalService } from '../../services/global.service'
import { ActivatedRoute } from '@angular/router'

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
    private route: ActivatedRoute
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
  isVisibleHistory : boolean = false;
  visibleDrawer : boolean = false;
  isVisibleStatus: boolean = false;
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
  };

  statusModel = {
    title: '',
    des: '',
    value: ''
  };

  headerId : any = ''

  ngOnInit() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const code = params.get('code');
        this.headerId = code;
        this.GetData(code);
      }
    });
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
  changeStatus(value: string){
    switch (value) {
      case '01':
        this.statusModel.title = "TRÌNH DUYỆT";
        this.statusModel.des = "Bạn có muốn Trình duyệt dữ liệu này?";
        break;
      case '02':
        this.statusModel.title = "YÊU CẦU CHỈNH SỬA";
        this.statusModel.des = "Bạn có muốn Yêu cầu chỉnh sửa lại dữ liệu này?";
        break;
      case '03':
        this.statusModel.title = "PHÊ DUYỆT";
        this.statusModel.des = "Bạn có muốn Phê duyệt dữ liệu này?";
        break;
      case '04':
        this.statusModel.title = "TỪ CHỐI";
        this.statusModel.des = "Bạn có muốn Từ chối dữ liệu này?";
        break;
    }
    this.isVisibleStatus = true
  }
  showHistoryAction(){
    this.isVisibleHistory = true;
  }
  handleOk(): void {
    this.isVisibleHistory = false;
    this.isVisibleStatus = false;
  }

  handleCancel(): void {
    this.isVisibleHistory = false;
    this.isVisibleStatus = false;
  }
  reCalculate(){
    this.GetData(this.model);
  }
  closeDrawer(){
    this.visibleDrawer = false;
  }
  getDataHeader(){
    this.visibleDrawer = true;
  }
}
