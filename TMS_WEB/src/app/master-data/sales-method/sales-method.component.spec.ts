import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesMethodComponent } from './sales-method.component';

describe('SalesMethodComponent', () => {
  let component: SalesMethodComponent;
  let fixture: ComponentFixture<SalesMethodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SalesMethodComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SalesMethodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
