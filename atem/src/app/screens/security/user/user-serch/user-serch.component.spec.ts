import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSerchComponent } from './user-serch.component';

describe('UserSerchComponent', () => {
  let component: UserSerchComponent;
  let fixture: ComponentFixture<UserSerchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserSerchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSerchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
