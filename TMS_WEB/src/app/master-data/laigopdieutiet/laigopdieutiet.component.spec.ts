import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaigopdieutietComponent } from './laigopdieutiet.component';

describe('LaigopdieutietComponent', () => {
  let component: LaigopdieutietComponent;
  let fixture: ComponentFixture<LaigopdieutietComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaigopdieutietComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LaigopdieutietComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
