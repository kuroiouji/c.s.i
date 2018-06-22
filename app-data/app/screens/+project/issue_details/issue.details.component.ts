import { 
    Component, 
    ViewChild,
    ElementRef
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
    CtmTextbox,
    CtmControlManagement,
    Md2Autocomplete,
    ConfirmChangeScreen,
    CONFIRM_CHANGE_SCREEN,
    ROLE_PERMISSION_TABLE,
    MESSAGE_DIALOG_TYPE,
    MESSAGE_DIALOG_RETURN_TYPE,
    DEFAULT_GROUP_CHECKER,
    PhaseAutoCompleteCriteria,
    USER_LOCAL_STORAGE
}                                       from "../../../common";
import { ProjectService }                 from "../project.service";
import { Ng2FileInputService, Ng2FileInputAction } from 'ng2-file-input';//<----importtt

import "style-loader!./issue.details.component.scss";
 
@Component({
  selector: "issue-details",
  templateUrl: "./issue.details.component.html"  
})
export class IssueDetailsComponent extends ABaseComponent implements ConfirmChangeScreen {
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
    @ViewChild("iscausephase") iscausephase: Md2Autocomplete;
    @ViewChild("isfoundphase") isfoundphase: Md2Autocomplete;
    @ViewChild('closeBtn') closeBtn: ElementRef

    private ctrlMgr: CtmControlManagement;
    private fix_rd_detail: CtmControlManagement;
    private controlDisabled: boolean = true;
    private tmpUserName: string;
    private resultData: CtmTableData;
    private screenDisabled:boolean;
    private HistoryData:any;
    private HistoryMember:any;
    private iss_acident:any;

    

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
            // root: "/s/project/issue/",
            screenParam: screenParam,
            commandMgr: {
            }            
        });  
        if(this.screenParam.is_id == undefined && this.screenParam.newData==false){
            this.screenParam.newData = true;
        }
        this.ctrlMgr = new CtmControlManagement(
            fb.group({
                "is_pj_id": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_no": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_module": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_description": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_title": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_expect_result": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_iss_id": [{value: null, disabled: true }],
                "iss_name": [{value: null, disabled: true }],
                "is_found_date": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_found_phase": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_cause_phase": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "is_deadline": [{value: null, disabled: true }],
                "is_found_by": [{value: null, disabled: true }, Validators.compose([Validators.required])]
            }),{
                translate: translate,
                screen: "ISSUE_DETAIL",
                mapping: {
                    "is_pj_id":"is_pj_id",
                    "is_no":"is_no",
                    "is_module":"is_module",
                    "is_description":"is_description",
                    "is_title":"is_title",
                    "is_expect_result":"is_expect_result",
                    "is_found_date":"is_found_date",
                    "is_found_by":"is_found_by",
                    "is_found_phase":"is_found_phase",
                    "is_cause_phase":"is_cause_phase"
                }
            }
        );

            this.fix_rd_detail = new CtmControlManagement(
                fb.group({
                    "detail": [{value: null, disabled: false }, Validators.compose([Validators.required])]
                }),{
                    translate: translate,
                    screen: "PDT021",
                    mapping: {
                        "detail": "detail",
                    }
                }
            );
  }

  get screenCommand() { return this.commandCtrl; }
  get screenChanged() : boolean {
    return this.ctrlMgr.changed == true;
  }
  
  initScreen(): Promise<Object> {
      this.loadData();
      
      let FoundPhase = new PhaseAutoCompleteCriteria();
      FoundPhase.pj_id = this.screenParam.is_pj_id;
     this.isfoundphase.loadDataByCriteria(FoundPhase);
     this.iscausephase.loadDataByCriteria(FoundPhase);

      return ;     
  }
  resetChanged() {
    this.ctrlMgr.resetChanged();
  }

  public confirmChange(): CONFIRM_CHANGE_SCREEN {
    return this.screenChanged ? CONFIRM_CHANGE_SCREEN.CONFIRM_UNSAVE : CONFIRM_CHANGE_SCREEN.CHANGE;
  }

  public confirmChangeResult(result: boolean) {
  }
  
  private ProjectChange(event){
        let FoundPhase = new PhaseAutoCompleteCriteria();
        FoundPhase.pj_id = event.value;
       this.isfoundphase.loadDataByCriteria(FoundPhase);
       this.iscausephase.loadDataByCriteria(FoundPhase);
  }
  
  private loadData(){
      console.log(this.screenParam.is_id);
    if(this.screenParam.is_id==undefined){
        this.screenParam.newData = true;
    }else{
        this.screenParam.newData = false;
    } 
    this.api.callApiController("api/ISSUE/GetIssueDetail", {
        type: "POST",
        data: {
            is_id: this.screenParam.is_id
        }
    }).then(
        (result) => {
            this.screenData = result;
            if (this.screenData != undefined || this.screenData != null) {
                this.ctrlMgr.data = this.screenData;
            }else if(this.screenParam != undefined){
                this.ctrlMgr.data = this.screenParam;
            }
                    
            $("#is_module").html(this.ctrlMgr.data.is_module);
            $("#is_description").html(this.ctrlMgr.data.is_description);
            $("#is_expect_result").html(this.ctrlMgr.data.is_expect_result);

            this.resetChanged();
        this.GetHistoryData();
      
            return true;   
        });
    
    this.controlDisabled = !this.screenParam.newData;
    this.ctrlMgr.setControlEnable(null, !this.controlDisabled);
    
    
    
  }
  private GetHistoryData()
  {   
      if(this.screenParam.is_id != undefined){
          this.api.callApiController("api/ISSUE/GetIssueHistory", {
              type: "POST",
              data: {
                  is_id : this.screenParam.is_id
              }
          }).then(
              (result) => {
                  this.HistoryData = result;
          });

          this.api.callApiController("api/ISSUE/GetIssueHistoryMember", {
              type: "POST",
              data: {
                  is_id : this.screenParam.is_id
              }
          }).then(
              (result) => {
                  this.HistoryMember = result;
          });
      }
  }
  private editCommand(){
    this.controlDisabled = false;
    this.ctrlMgr.setControlEnable(null, !this.controlDisabled);
  }
    ClickModal(str){
        this.iss_acident = str;
        $('.to_status').html(str);
        $('.from_status').html(this.ctrlMgr.data.iss_name);
        $('.ref_no').html(this.ctrlMgr.data.is_no);
    }

    ClickSaveModal(){
        if(this.iss_acident=="Re-debug"
            ||this.iss_acident=="Miss"
            ||this.iss_acident=="Cancel"
            ||this.iss_acident=="Fix"){
            if (this.fix_rd_detail.validate()) {
                this.api.callApiController("api/ISSUE/UpdateIssueByNextPhase", {
                    type: "POST",
                    data: {
                        is_id : this.screenParam.is_id,
                        acident_phase : this.iss_acident,
                        us_username: this.getUserName,
                        detail : this.fix_rd_detail.data.detail
                    }
                }).then(
                    (result) => {
                        let criteria = this.ctrlMgr.data;
                        
                        this.closeBtn.nativeElement.click();
                        this.loadData();
                    });
            }
        }
    }
    CheckDisable(input:string){
        let iss_name = this.ctrlMgr.data.iss_name;
        if(iss_name=="Re-debug"
            || iss_name=="Miss"
            || iss_name=="Cancel"){
                return false;
            }else{

                if(input=="Miss"){
                    return this.hasPermissionISSMiss();
                }else if(input=="Cancel"){
                    return this.hasPermissionISSCancel();
                }else if(input=="Edit"){
                    return this.hasPermissionISSEdit();
                }
            }
    }
    onUpdateCommand() {
        if (this.ctrlMgr.validate()) {
            this.message.openMessageDialog(
                this.translate.instant("CLC003", "MESSAGE"),
                MESSAGE_DIALOG_TYPE.QUESTION
            ).then((type: MESSAGE_DIALOG_RETURN_TYPE) => {
                    let iss_id = this.ctrlMgr.data.is_iss_id;
                    let url = "api/Issue/UpdateIssueDetail";
                    if (this.screenParam.newData == true) {
                        url = "api/Issue/InsertIssue";
                        iss_id = 1;
                    }

                    this.api.callApiController(url, {
                        type: "POST",
                        data: {
                                is_id               :this.screenParam.is_id,
                                is_pj_id            :this.ctrlMgr.data.is_pj_id,
                                is_no               :this.ctrlMgr.data.is_no,
                                is_module           :this.ctrlMgr.data.is_module,
                                is_title            :this.ctrlMgr.data.is_title,
                                iss_name            :this.ctrlMgr.data.iss_name,
                                is_description      :this.ctrlMgr.data.is_description,
                                is_expect_result    :this.ctrlMgr.data.is_expect_result,
                                is_found_by         :this.ctrlMgr.data.is_found_by,
                                is_found_date       :this.ctrlMgr.data.is_found_date,
                                is_found_phase      :this.ctrlMgr.data.is_found_phase,
                                is_cause_phase      :this.ctrlMgr.data.is_cause_phase,
                                is_deadline         :this.ctrlMgr.data.is_deadline
                               
                        }
                    }).then(
                        result => {
                            // console.log("success");
                            if (this.screenParam.newData == true) {
                                this.screenParam.is_id = undefined;
                                this.backToRoot();
                            }else{
                                this.loadData();
                            }
                        }
                    );
                
            });
        }
    }
    onCancelCommand(){
        if(this.screenParam.newData == true){
            this.backToRoot();
        }else{
            this.loadData();
        }
    }
    private getUserName(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); 
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          return usr.userName;
        }
    }

}
