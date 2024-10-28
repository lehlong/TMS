import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MgOpinionListComponent } from './mg-opinion-list.component';

describe('MgOpinionListComponent', () => {
  let component: MgOpinionListComponent;
  let fixture: ComponentFixture<MgOpinionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MgOpinionListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MgOpinionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
