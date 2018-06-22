import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ReportComponent }                  from "./report.component";

// noinspection TypeScriptValidateTypes
export const routes: Routes = [
  {
    path: "",
    component: ReportComponent
  }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
