import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateListTablesEditComponent } from './template-list-tables-edit.component';

describe('TemplateListTablesEditComponent', () => {
  let component: TemplateListTablesEditComponent;
  let fixture: ComponentFixture<TemplateListTablesEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateListTablesEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TemplateListTablesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
