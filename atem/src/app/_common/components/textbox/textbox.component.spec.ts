import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmTextboxComponent } from './textbox.component';

describe('CtmTextboxComponent', () => {
  let component: CtmTextboxComponent;
  let fixture: ComponentFixture<CtmTextboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmTextboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmTextboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
