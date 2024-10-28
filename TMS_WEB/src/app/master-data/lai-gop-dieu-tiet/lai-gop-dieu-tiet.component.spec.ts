import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaiGopDieuTietComponent } from './lai-gop-dieu-tiet.component';

describe('LaiGopDieuTietComponent', () => {
  let component: LaiGopDieuTietComponent;
  let fixture: ComponentFixture<LaiGopDieuTietComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaiGopDieuTietComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LaiGopDieuTietComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
