import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { CtmModule } from "../../../_common";

import { LoginComponent } from './login.component';
import { routing } from "./login.routing";

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    routing,

    CtmModule
  ],
  declarations: [LoginComponent]
})
export class LoginModule { }
