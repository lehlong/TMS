import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListAuditEditComponent } from './list-audit-edit.component';

describe('ListAuditEditComponent', () => {
  let component: ListAuditEditComponent;
  let fixture: ComponentFixture<ListAuditEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListAuditEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListAuditEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
