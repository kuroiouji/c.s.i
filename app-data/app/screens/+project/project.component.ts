import { 
  Component, 
  ViewChild 
}                                       from "@angular/core";
import {
    Router,
    ActivatedRoute
}                                       from "@angular/router";
import { 
    FormBuilder, 
    Validators 
}                                       from "@angular/forms";

import {
    ABaseComponent,
    BaThemeSpinner,
    ApiService,
    CtmScreenCommand,
    CtmTranslateService,
    CtmMessageService,
    CtmCommandService,
    CtmTableService,
    CtmTableData,
    CtmControlManagement,
    USER_LOCAL_STORAGE,
    MESSAGE_DIALOG_TYPE,
    MESSAGE_DIALOG_RETURN_TYPE,
    ROLE_PERMISSION_TABLE    
}                                       from "../../common";


import { ProjectService }                 from "./project.service";

import "style-loader!./project.component.scss";

@Component({
  selector: "project",
  templateUrl: "./project.component.html"  
})
export class ProjectComponent extends ABaseComponent {
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;

    private criteriaCtrlMgr: CtmControlManagement;
    private resultData: object;

    constructor(
        router: Router,
        route: ActivatedRoute,
        spinner: BaThemeSpinner,
        translate: CtmTranslateService,
        message: CtmMessageService,
        command: CtmCommandService,
        
screenParam: ProjectService,

        private fb: FormBuilder,
        private api: ApiService,
        private table: CtmTableService
    ) {
          super(router, route, spinner, translate, message, command, {
            screenParam: screenParam,
            commandMgr: {
                addCommand: true
            }            
        });

        
        this.criteriaCtrlMgr = new CtmControlManagement(
            fb.group({
                "PjNameOwner" : [null]
            })
            
            // , {
            //     translate: translate,
            //     screen: "SCS010",
            //     mapping: null
            // }
        );
    }
    
    get screenCommand() { return this.commandCtrl; }
     screenChanged: boolean = false;

    initScreen(): Promise<Object> {
       
        return;
    }
    
    resetChanged() { }

    private onSearch() {
      let criteria = this.criteriaCtrlMgr.data;
      this.api.callApiController(
                         "api/PROJECT/GetProjectList", {
                        type: "POST",
                        data: {
                            pj_name : criteria.PjNameOwner,
                            us_username : this.getUsername()
                        }
                    }).then(
                        result => { 
                            this.resultData = result;
                        });
    }

    ClickIssue(row){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          if (usr != undefined) {
            let data = {  
                userName: usr.userName,
                displayName: usr.displayName,
                groupID:  usr.groupID,
                pj_id: row.pj_id,
                pj_name : row.pj_name
            };
            localStorage.removeItem(USER_LOCAL_STORAGE);
            localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(data));
            this.screenParam.pj_id = row.pj_id;
            this.router.navigate(["/s/project/issue"]);
            
          }
        }
    }

    getUsername(){
        
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          if (usr != undefined) {
            return usr.userName;
          }
        }
    }
    ClickDetail(row){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          if (usr != undefined) {
            let data = {  
                userName: usr.userName,
                displayName: usr.displayName,
                groupID:  usr.groupID,
                pj_id: row.pj_id,
                pj_name : row.pj_name
            };
            localStorage.removeItem(USER_LOCAL_STORAGE);
            localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(data));
            this.screenParam.newData = false;
            this.router.navigate(["/s/project/pd"]);
       
            
          }
        }
        
                            
        
         

    }

    ClickReport(row){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
            let usr = JSON.parse(loc);
            if (usr != undefined) {
                let data = {  
                    userName: usr.userName,
                    displayName: usr.displayName,
                    groupID:  usr.groupID,
                    pj_id: row.pj_id,
                    pj_name : row.pj_name
                };
                localStorage.removeItem(USER_LOCAL_STORAGE);
                localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(data));
                this.router.navigate(["/s/report"]);
            }
        }
    }

    NewProject(){
        this.screenParam.newData = true;
        this.router.navigate(["/s/project/pd"]);
    }
    

    private isHasCreate(){
        return this.hasPermissionCreatePro();
    }
}
