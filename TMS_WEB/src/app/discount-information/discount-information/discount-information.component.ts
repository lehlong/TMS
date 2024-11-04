import { Component } from '@angular/core';
import { GlobalService } from '../../services/global.service';
import { ShareModule } from '../../shared/share-module';
import { ActivatedRoute } from '@angular/router';
import { GoodsService } from '../../services/master-data/goods.service';
import { DiscountInformationService } from '../../services/discount-information/discount-information.service';

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
    private _goodsService: GoodsService,
    private globalService: GlobalService,
    private route: ActivatedRoute,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Kết quả tính toán đầu ra',
        path: 'calculate-result',
      },
    ])
  }
//  lstGoods: any[] = []
  data: any = {
    lstGoods : [],
    discount: []
  }

  ngOnInit() {
    this.getAll()
    // this.getAllGoods()
  }

  getAll() {
    this._service.getAll().subscribe({
      next: (data) => {
        console.log(data.lstGoods);
        this.data = data
        console.log(this.data);

      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  // getAllGoods() {
  //   this._goodsService.getall().subscribe({
  //     next: (data) => {
  //       this.data.lstGoods = data
  //     },
  //     error: (response) => {
  //       console.log(response)
  //     },
  //   })
  // }
}
