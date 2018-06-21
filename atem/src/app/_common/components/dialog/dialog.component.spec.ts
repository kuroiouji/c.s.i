import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmDialogComponent } from './dialog.component';

describe('CtmDialogComponent', () => {
  let component: CtmDialogComponent;
  let fixture: ComponentFixture<CtmDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
