import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmAutocompleteComponent } from './autocomplete.component';

describe('CtmAutocompleteComponent', () => {
  let component: CtmAutocompleteComponent;
  let fixture: ComponentFixture<CtmAutocompleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmAutocompleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
