import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MgListTablesGroupsComponent } from './mg-list-tables-groups.component';

describe('MgListTablesGroupsComponent', () => {
  let component: MgListTablesGroupsComponent;
  let fixture: ComponentFixture<MgListTablesGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MgListTablesGroupsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MgListTablesGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
