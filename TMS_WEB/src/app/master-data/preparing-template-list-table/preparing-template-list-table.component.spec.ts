import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreparingTemplateListTableComponent } from './preparing-template-list-table.component';

describe('PreparingTemplateListTableComponent', () => {
  let component: PreparingTemplateListTableComponent;
  let fixture: ComponentFixture<PreparingTemplateListTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PreparingTemplateListTableComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PreparingTemplateListTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
