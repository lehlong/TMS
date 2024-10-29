import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculateResultListComponent } from './calculate-result-list.component';

describe('CalculateResultListComponent', () => {
  let component: CalculateResultListComponent;
  let fixture: ComponentFixture<CalculateResultListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalculateResultListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CalculateResultListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
