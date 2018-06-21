import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmDatePickerComponent } from './datepicker.component';

describe('CtmDatePickerComponent', () => {
  let component: CtmDatePickerComponent;
  let fixture: ComponentFixture<CtmDatePickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmDatePickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmDatePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
