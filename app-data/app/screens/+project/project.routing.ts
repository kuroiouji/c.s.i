import { ModuleWithProviders }      from "@angular/core";
import { Routes, RouterModule }     from "@angular/router";

import {
    RoleData,
    PjRoleData,
    ROLE_PROJECT,
    ROLE_PERMISSION,
    ConfirmChangeScreenGuard
}                                   from "../../common";

import { ProjectComponent }             from "./project.component";
import { ProjectDetailsComponent }      from "./project_details/project.details.component";
import { IssueComponent }               from "./issue/issue.component";
import { IssueDetailsComponent }        from "./issue_details/issue.details.component";
import { uploadComponent }        from "./upload/upload.component";

export const routes: Routes = [
  {
      path: "",
      redirectTo: "p",
      pathMatch: "full"
    },
  {
      path: "p",
      component: ProjectComponent,
      data: { 
        "ProjectRole": new PjRoleData(
                  { permissions:[ROLE_PROJECT.CREATE_PRO]}
        )
      }
  },
  {
      path: "pd",
      component: ProjectDetailsComponent,
      data: { 
        "ProjectRole": new PjRoleData(
                  { pj_id:"1",permissions:[ROLE_PROJECT.EDIT_PROJECT,ROLE_PROJECT.DELETE_PRO],
                  
                }
        )
      }
  },
  {   
        path : "u",
        component: uploadComponent
  },
  {   
        path : "issue",
        children : [
        {
                path: "",
                component: IssueComponent,
                pathMatch: "full",
                data: { 
                  "ProjectRole": new PjRoleData(
                            { permissions:[ ROLE_PROJECT.EDIT_ISSUE
                                            ,ROLE_PROJECT.PLAN
                                            ,ROLE_PROJECT.PROCESS_FIX
                                            ,ROLE_PROJECT.CLOSE
                                            ,ROLE_PROJECT.CHECK
                                            ,ROLE_PROJECT.MISS
                                            ,ROLE_PROJECT.CANCEL
                                            ,ROLE_PROJECT.RE_DEBUG]}
                  )
                }
                
        },
        {   
                path : "d",
                component: IssueDetailsComponent,
                data: { 
                  "ProjectRole": new PjRoleData(
                            { permissions:[
                              ROLE_PROJECT.CANCEL
                              ,ROLE_PROJECT.MISS]}
                  )
                }
        }
        ]
    }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
