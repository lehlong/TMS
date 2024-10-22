import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculateResultComponent } from './calculate-result.component';

describe('CalculateResultComponent', () => {
  let component: CalculateResultComponent;
  let fixture: ComponentFixture<CalculateResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalculateResultComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CalculateResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
