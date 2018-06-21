import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSerchComponent } from './user-serch/user-serch.component';
import { UserDetailComponent } from './user-detail/user-detail.component';

import { routing } from "./user.routing";
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CtmModule } from '../../../_common';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    routing,
    CtmModule
  ],
  declarations: [UserSerchComponent, UserDetailComponent]
})
export class UserModule { }
