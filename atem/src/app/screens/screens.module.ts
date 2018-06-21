import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CtmModule } from "../_common";

import { routing } from "./screens.routing";

import { ScreensComponent } from "./screens.component";

@NgModule({
  imports: [
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
		FormsModule,
    routing,

    CtmModule
  ],
  declarations: [
    ScreensComponent
  ]
})
export class ScreensModule { }
