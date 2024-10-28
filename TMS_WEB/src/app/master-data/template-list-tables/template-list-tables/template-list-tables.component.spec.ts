import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateListTablesComponent } from './template-list-tables.component';

describe('TemplateListTablesComponent', () => {
  let component: TemplateListTablesComponent;
  let fixture: ComponentFixture<TemplateListTablesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateListTablesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TemplateListTablesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
