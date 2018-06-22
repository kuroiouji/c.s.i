import { NgModule }                 from "@angular/core";
import { CommonModule }             from "@angular/common";
import { HttpModule }               from "@angular/http";
import { 
    FormsModule, 
    ReactiveFormsModule 
}                                   from "@angular/forms";

import { CtmModule }                from "../../common";

import { ProjectService }              from "./project.service";

import { ProjectComponent }      from "./project.component";
import { ProjectDetailsComponent }      from "./project_details/project.details.component";
import { IssueComponent }      from "./issue/issue.component";
import { IssueDetailsComponent }      from "./issue_details/issue.details.component";
import { uploadComponent }      from "./upload/upload.component";


import { routing }                  from "./project.routing";



import { Ng2FileInputModule } from 'ng2-file-input'; // <-- import the module
import { DragulaModule } from 'ng2-dragula'; //<-- import drag and drop
import { Daterangepicker } from 'ng2-daterangepicker'; //<-- date picker
@NgModule({
  imports: [
      HttpModule,
      CommonModule,
      ReactiveFormsModule,
      FormsModule,
      routing,

      CtmModule,
      Ng2FileInputModule.forRoot(), // <-- include it in your app module
      DragulaModule, // drag and drop
      Daterangepicker // date picker
  ],
  declarations: [
      ProjectComponent,
      ProjectDetailsComponent,
      IssueComponent,
      IssueDetailsComponent,
      uploadComponent
  ],
  providers: [
      ProjectService
  ]
})
export class ProjectModule {}
