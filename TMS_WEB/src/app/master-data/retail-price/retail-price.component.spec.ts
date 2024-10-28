import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RetailPriceComponent } from './retail-price.component';

describe('RetailPriceComponent', () => {
  let component: RetailPriceComponent;
  let fixture: ComponentFixture<RetailPriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RetailPriceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RetailPriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
