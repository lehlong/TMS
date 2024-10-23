import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeSoMatHangComponent } from './he-so-mat-hang.component';

describe('HeSoMatHangComponent', () => {
  let component: HeSoMatHangComponent;
  let fixture: ComponentFixture<HeSoMatHangComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeSoMatHangComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HeSoMatHangComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
