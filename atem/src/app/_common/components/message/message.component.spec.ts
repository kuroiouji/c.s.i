import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmMessageComponent } from './message.component';

describe('CtmMessageComponent', () => {
  let component: CtmMessageComponent;
  let fixture: ComponentFixture<CtmMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
