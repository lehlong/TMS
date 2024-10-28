import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TinhToanDauRaComponent } from './tinh-toan-dau-ra.component';

describe('TinhToanDauRaComponent', () => {
  let component: TinhToanDauRaComponent;
  let fixture: ComponentFixture<TinhToanDauRaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TinhToanDauRaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TinhToanDauRaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
