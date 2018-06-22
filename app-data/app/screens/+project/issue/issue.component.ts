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
    USER_LOCAL_STORAGE,
    textDateLocale,
    PhaseAutoCompleteCriteria
}                                       from "../../../common";
import { ProjectService }                 from "../project.service";
import { DaterangepickerConfig } from 'ng2-daterangepicker'; // <----- import datepicker
import * as moment from 'moment/moment'

import "style-loader!./issue.component.scss";
 
@Component({
  selector: "issue",
  templateUrl: "./issue.component.html"  
})
export class IssueComponent extends ABaseComponent implements ConfirmChangeScreen {
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
    @ViewChild('closeBtn') closeBtn: ElementRef
    @ViewChild("ProjectMem") ProjectMem: Md2Autocomplete;

    private ctrlMgr: CtmControlManagement;
    private fix_rd_detail: CtmControlManagement;
    private plan_detail: CtmControlManagement;
    private controlDisabled: boolean = true;
    private tmpUserName: string;
    private resultData: any;
    public  singleDate: any; // declare var for datepicker
    public  historyRow: any;
    public  historyMem: any;
    private is_id:number;
    private iss_acident:any; // iss_id of next phase 
    private pj_name;
    

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
        private table: CtmTableService,
        private daterangepickerOptions: DaterangepickerConfig // date picker
  ) {
      
      super(router, route, spinner, translate, message, command, {
            root: "/s/project/issue",
            screenParam: screenParam,
            commandMgr: {
                // addCommand: true,  
                // editCommand: true,
                // updateCommand: true,
                cancelCommand: true,
                backUrl: "/s/project/p"
            }            
        });  
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); //-- start set data for get issue table by pj_id--------------//
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          
            if(usr.pj_id != undefined && this.screenParam.is_pj_id == undefined){
                this.screenParam.is_pj_id = usr.pj_id;
            }else if(usr.pj_id == undefined){
                this.router.navigate(["/s/project/p"]);
            }else{
                window.location.reload();
            }
          }
          //------------------------------end set data for get issue table by pj_id -------------------------//
        this.daterangepickerOptions.settings = { //---------set daterange -------//
            locale: { format: 'DD-MMM-YYYY' },
            alwaysShowCalendars: false
        };
        // this.singleDate = Date.now(); //----- initial data for datepicker ---//


        this.ctrlMgr = new CtmControlManagement(
            fb.group({
                "search_input": [{value: null, disabled: false }],
                "search_priority": [{value: null, disabled: false }],
                "search_status": [{value: null, disabled: false }]
            })
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

        this.plan_detail = new CtmControlManagement(
            fb.group({
                "plan_user": [{value: null, disabled: false }, Validators.compose([Validators.required])],
                "Serverity": [{value: null, disabled: false }, Validators.compose([Validators.required])],
                "Priority": [{value: null, disabled: false }, Validators.compose([Validators.required])]
            }), {
                translate: translate,
                screen: "PDT021",
                mapping: {
                    "plan_user": "plan_user",
                    "Serverity": "Serverity",
                    "Priority": "Priority"
                }
            }
        );
       
         
        
  }
  private singleSelect(value: any) {
    this.singleDate = value.start;
}
  public singlePicker = {
    singleDatePicker: true,
    showDropdowns: true,
    opens: "left"
  }
  private phase={
      1:'Open',
      2:'Plan',
      3:'Process',
      4:'Fix',
      5:'Check',
      6:'Close',
      7:'Re-debug',
      8:'Miss',
      9:'Cancel'
  }
  get screenCommand() { return this.commandCtrl; }
  get screenChanged() : boolean {
    return this.ctrlMgr.changed == true;
  }
  
  initScreen(): Promise<Object> {
    this.pj_name = this.getProjectName();
    
    return ;
  }
  resetChanged() {
    this.ctrlMgr.resetChanged();
  }

  public confirmChange(): CONFIRM_CHANGE_SCREEN {
    return this.screenChanged ? CONFIRM_CHANGE_SCREEN.CONFIRM_UNSAVE : CONFIRM_CHANGE_SCREEN.CHANGE;
  }
  public confirmChangeResult(result: boolean) {
    if (result == true) {
      this.screenParam.BranchName = null;
    }
  }

    ClickHistory(is_id,is_no)
    {   $('.is_no_small').html('('+is_no+')');
        if(is_id != undefined){
            this.api.callApiController("api/ISSUE/GetIssueHistory", {
                type: "POST",
                data: {
                    is_id : is_id
                }
            }).then(
                (result) => {
                    this.historyRow =  result;
            });
            
            this.api.callApiController("api/ISSUE/GetIssueHistoryMember", {
                type: "POST",
                data: {
                    is_id : is_id
                }
            }).then(
                (result) => {
                    this.historyMem =  result;
            });
        }
    }
    ClickDetails(is_id){
        this.screenParam.is_id = is_id;
        this.router.navigate(["/s/project/issue/d"]);
    }
    
    ClickModal(row){
        this.plan_detail.clear(true);
        this.fix_rd_detail.clear(true);
        this.is_id = row.is_id;
        this.iss_acident = false;
        
      let FoundPhase = new PhaseAutoCompleteCriteria();
      FoundPhase.pj_id = this.screenParam.is_pj_id;
        
        this.ProjectMem.loadDataByCriteria(FoundPhase);
        $("#is_module").html(row.is_module);
        $("#is_description").html(row.is_description);
        $("#is_expect_result").html(row.is_expect_result);
        for(var i=1;i<10;i++){
            if(row.iss_name==this.phase[i]){
                this.iss_acident = this.phase[i+1];
        
                $('.to_status').html(this.phase[i+1]);
                $('.from_status').html(row.iss_name);
                $('.ref_no').html(row.is_no);
                var str = '';
                if(this.phase[i+1]=='Fix'){ // if next phase == fix
                    $('.plan_detail').hide();
                    $('.fix_rd_detail').show();
                    $('.detail_label').html('Solution');
                }else if(this.phase[i+1]=='Re-debug'){ // if next phase == re-debug
                    $('.fix_rd_detail').show();
                    $('.plan_detail').hide();
                    $('.detail_label').html('Remark');
                }else if(this.phase[i+1]=='Plan'){// if next phase == plan
                    $('.fix_rd_detail').hide();
                    $('.plan_detail').show();
                }else{
                    $('.fix_rd_detail').hide();
                    $('.plan_detail').hide();
                }
            }
        }
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
                        is_id : this.is_id,
                        acident_phase : this.iss_acident,
                        detail : this.fix_rd_detail.data.detail,
                        us_username : this.getUserName()
                    }
                }).then(
                    (result) => {
                        let criteria = this.ctrlMgr.data;
                        
                        this.loadData(criteria);
                        this.closeBtn.nativeElement.click();
                    });
             }
        }else if(this.iss_acident=="Plan"){
            
            if (this.plan_detail.validate()) {
                this.api.callApiController("api/ISSUE/UpdateIssueByNextPhase", {
                    type: "POST",
                    data: {
                        is_id : this.is_id,
                        is_st_id : this.plan_detail.data.Serverity,
                        is_pt_id : this.plan_detail.data.Priority,
                        is_plan_finish_date : this.singleDate,
                        us_planto : this.plan_detail.data.plan_user ,
                        us_username : this.getUserName()
                    }
                    
                }).then(
                    (result) => {
                        let criteria = this.ctrlMgr.data;
                        
                        this.loadData(criteria);
                        this.closeBtn.nativeElement.click();
                    });
            }
        }else{
                this.api.callApiController("api/ISSUE/UpdateIssueByNextPhase", {
                    type: "POST",
                    data: {
                        is_id : this.is_id,
                        us_username : this.getUserName()
                    }
                }).then(
                    (result) => {
                        let criteria = this.ctrlMgr.data;
                        
                        this.loadData(criteria);
                        this.closeBtn.nativeElement.click();
                    });
        }
    }

    NewIssue(){
        this.screenParam.newData = true;

        this.router.navigate(["/s/project/issue/d"]);

    }
    private checkDisable(row){
        
        if(row.iss_name=='Open'){
            return this.hasPermissionISSPlan(); // check he has perrmission to change status to plan
        }else if(row.iss_name=='Plan'){
            let CheckUs = false;
            if(row.plan_user==this.getUserName()){
                CheckUs = true;
            }
            return this.hasPermissionISSFix() && CheckUs;
        }else if(row.iss_name=='Process'){
            let CheckUs = false;
            if(row.plan_user==this.getUserName()){
                CheckUs = true;
            }
            return this.hasPermissionISSFix() && CheckUs;
        }else if(row.iss_name=='Fix'){
            return this.hasPermissionISSCheck();
        }else if(row.iss_name=='Check'){
            return this.hasPermissionISSClose();
        }else if(row.iss_name=='Close'){
            return this.hasPermissionISSRedebug();
        }else{
            return false;
        }
    }
    private loadData(criteria): Promise<Object> {
        if (this.screenParam.is_pj_id!=undefined) {
            return  this.api.callApiController("api/ISSUE/GetIssueList", {
                        type: "POST",
                        data: {
                            is_pj_id : this.screenParam.is_pj_id,
                            is_no : criteria.search_input,
                            is_title : criteria.search_input,
                            is_pt_id : criteria.search_priority,
                            is_iss_id : criteria.search_status
                        }
                    }).then(
                        (result) => {
                            this.resultData = result;

                            return true;   
                        });
        }

        return;
    }
    private onSearch() {
        let criteria = this.ctrlMgr.data;
        
        this.loadData(criteria);
      }
    private getUserName(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); 
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          return usr.userName;
        }
    }
    private getProjectName(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); 
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          return usr.pj_name;
        }
    }
}
