import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MapPointCustomerGoodsComponent } from './map-point-customer-goods.component';

describe('MapPointCustomerGoodsComponent', () => {
  let component: MapPointCustomerGoodsComponent;
  let fixture: ComponentFixture<MapPointCustomerGoodsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MapPointCustomerGoodsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MapPointCustomerGoodsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
