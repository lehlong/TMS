import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateReportComponent } from './template-report.component';

describe('TemplateReportComponent', () => {
  let component: TemplateReportComponent;
  let fixture: ComponentFixture<TemplateReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateReportComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TemplateReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
