import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MucGiamPhoThongComponent } from './muc-giam-pho-thong.component';

describe('MucGiamPhoThongComponent', () => {
  let component: MucGiamPhoThongComponent;
  let fixture: ComponentFixture<MucGiamPhoThongComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MucGiamPhoThongComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MucGiamPhoThongComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
