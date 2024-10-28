import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateListTablesPreviewComponent } from './template-list-tables-preview.component';

describe('TemplateListTablesPreviewComponent', () => {
  let component: TemplateListTablesPreviewComponent;
  let fixture: ComponentFixture<TemplateListTablesPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateListTablesPreviewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TemplateListTablesPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
