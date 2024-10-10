import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditPeriodListTablesComponent } from './audit-period-list-tables.component';

describe('AuditPeriodListTablesComponent', () => {
  let component: AuditPeriodListTablesComponent;
  let fixture: ComponentFixture<AuditPeriodListTablesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditPeriodListTablesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuditPeriodListTablesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
