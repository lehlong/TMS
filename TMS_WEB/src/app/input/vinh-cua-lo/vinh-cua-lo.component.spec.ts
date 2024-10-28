import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VinhCuaLoComponent } from './vinh-cua-lo.component';

describe('VinhCuaLoComponent', () => {
  let component: VinhCuaLoComponent;
  let fixture: ComponentFixture<VinhCuaLoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VinhCuaLoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VinhCuaLoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
