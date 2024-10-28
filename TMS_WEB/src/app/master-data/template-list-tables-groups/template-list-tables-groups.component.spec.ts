import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateListTablesGroupsComponent } from './template-list-tables-groups.component';

describe('TemplateListTablesGroupsComponent', () => {
  let component: TemplateListTablesGroupsComponent;
  let fixture: ComponentFixture<TemplateListTablesGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateListTablesGroupsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TemplateListTablesGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
