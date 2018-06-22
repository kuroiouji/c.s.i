import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSerchComponent } from './user-serch/user-serch.component';
import { UserDetailComponent } from './user-detail/user-detail.component';

import { routing } from "./user.routing";
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CtmModule } from '../../../_common';
import { UserGroupSearchComponent } from './user-group-search/user-group-search.component';
import { UserGroupDetailComponent } from './user-group-detail/user-group-detail.component';
import { ScreenParam } from './user.screen-param';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    routing,
    CtmModule
  ],
  declarations: [
    UserSerchComponent, 
    UserDetailComponent, 
    UserGroupSearchComponent, 
    UserGroupDetailComponent
  ],
  providers: [ScreenParam]
})
export class UserModule { }
