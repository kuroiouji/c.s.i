import { NgModule }             from "@angular/core";
import { CommonModule }         from "@angular/common";
import { HttpModule }           from "@angular/http";
import {
    FormsModule,
    ReactiveFormsModule
}                               from "@angular/forms";

import {
    CtmModule    
}                               from "../../common";

import { DashboardComponent }       from "./dashboard.component";
import { routing }              from "./dashboard.routing";

import { ChartModule }            from 'angular2-highcharts'; //highchart
@NgModule({
  imports: [
      HttpModule,
      CommonModule,
      ReactiveFormsModule,
      FormsModule,
      routing,
      ChartModule.forRoot(require('highcharts')),//highchart
      CtmModule
  ],
  declarations: [
      
	DashboardComponent
  ],
  providers: [
  ]
})
export class DashboardModule {}
