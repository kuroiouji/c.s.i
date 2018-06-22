import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserGroupSearchComponent } from './user-group-search.component';

describe('UserGroupSearchComponent', () => {
  let component: UserGroupSearchComponent;
  let fixture: ComponentFixture<UserGroupSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserGroupSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserGroupSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
