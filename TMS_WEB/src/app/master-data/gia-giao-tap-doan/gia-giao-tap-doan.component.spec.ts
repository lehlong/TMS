import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiaGiaoTapDoanComponent } from './gia-giao-tap-doan.component';

describe('GiaGiaoTapDoanComponent', () => {
  let component: GiaGiaoTapDoanComponent;
  let fixture: ComponentFixture<GiaGiaoTapDoanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiaGiaoTapDoanComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GiaGiaoTapDoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
