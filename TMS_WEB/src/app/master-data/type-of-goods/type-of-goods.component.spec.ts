import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeOfGoodsComponent } from './type-of-goods.component';

describe('TypeOfGoodsComponent', () => {
  let component: TypeOfGoodsComponent;
  let fixture: ComponentFixture<TypeOfGoodsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TypeOfGoodsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TypeOfGoodsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
