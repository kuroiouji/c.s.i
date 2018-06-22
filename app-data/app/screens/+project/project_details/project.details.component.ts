import {
    Component,
    ViewChild
} from "@angular/core";
import {
    Router,
    ActivatedRoute
} from "@angular/router";
import {
    FormBuilder,
    Validators
} from "@angular/forms";

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
    textFormat
} from "../../../common";
import { ProjectService } from "../project.service";

import { DragulaService } from 'ng2-dragula/ng2-dragula'; //<------ import drag and drop
import { DaterangepickerConfig } from 'ng2-daterangepicker'; // <----- import datepicker
import * as moment from 'moment/moment' // <--- import moment for datepicker
import "style-loader!./project.details.component.scss";


@Component({
    selector: "project-details",
    templateUrl: "./project.details.component.html"
})
export class ProjectDetailsComponent extends ABaseComponent implements ConfirmChangeScreen {
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
    // @ViewChild("BranchID") BranchID: CtmTextbox;
    @ViewChild("BrandCode") BrandCode: Md2Autocomplete;
    @ViewChild("ProvinceID") ProvinceID: Md2Autocomplete;
    
    private ctrlMgr: CtmControlManagement;
    private ctrlMgr2: CtmControlManagement;
    private controlDisabled: boolean = true;
    private tmpUserName: string;
    private userChanged: boolean;
    private MemberCtrlMgr: CtmControlManagement;
    private newInfo = {user:"",
        priority:"",
        position:""};
        private MemberData: Array<Object>;
        private UserAll;
        private Data: Array<Object>;
        // public PhaseOptioni: Array<string>;
        // public PhaseSelectedi: Array<string>;
        public PhaseOption;
        public PhaseSelected;
       
    public mainInput = {
            start: moment(),
            end: (moment().add('1','day') )
        }
    
        

        
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

        private dragulaService: DragulaService, // drag and drop
        private daterangepickerOptions: DaterangepickerConfig // date picker
        

    ) {
        
        
        super(router, route, spinner, translate, message, command, {
            //root: "/s/master/b/s",
            screenParam: screenParam,
            commandMgr: {
                addCommand: true,
                editCommand: true,
                updateCommand: true,
                cancelCommand: true,
                backUrl: "/s/project/p"
            }      
        });

        // if(this.screenParam.newData){
        //     this.mainInput.start = moment(),
        //     this.mainInput.end = (moment().add('1','day') )
        // }

        let data = localStorage.getItem(USER_LOCAL_STORAGE);
        if(data != undefined){
            let usr = JSON.parse(data)
                // console.log(usr);
            if(usr.pj_id == undefined || usr.pj_id == null){
                if(!this.screenParam.newData){
                    this.router.navigate(["/s/project/p"]); 
                }
            }
        }
        this.PhaseOption = ['Analysis&Design', 'Construction', 'Waratee', 'Check','Asign'];
        this.PhaseSelected = ['Analysis', 'Design','Code','Test','Maintain'];
        this.ctrlMgr = new CtmControlManagement(
            fb.group({
                "pj_id": {value: null},
                "pj_project_code": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "pj_name": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "pj_description": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "us_id": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "pjs_id": [{value: null, disabled: true }, Validators.compose([Validators.required])],
                "pj_start_date": [{disabled: true }],
                "pj_end_date": [{value: null, disabled: true }, Validators.compose([Validators.required])]
            }), {
                translate: translate,
                screen: "PROJECT",
                mapping: {
                    "pj_project_code": "pj_project_code",
                    "pj_name": "pj_name",
                    "us_id": "us_id",
                    "pjs_id": "pjs_id",
                    "pj_end_date": "pj_end_date"
                }
            }
        );
        this.MemberCtrlMgr = new CtmControlManagement(
            fb.group({
                "us_id": [{value: null, disabled: true }, Validators.compose([Validators.required])]
            }), null
        );


        this.daterangepickerOptions.settings = {
            locale: { format: 'DD-MMM-YYYY' },
            alwaysShowCalendars: false,
            ranges: {
               'Next Month': [ moment(),moment().add(1, 'month')],
               'Next 3 Months': [ moment(),moment().add(4, 'month')],
               'Next 6 Months': [ moment(),moment().add(6, 'month')],
               'Next 12 Months': [ moment(),moment().add(12, 'month')],
            }
        };
        dragulaService.dropModel.subscribe((value) => {
            this.onDropModel(value.slice(1));
          });
        dragulaService.removeModel.subscribe((value) => {
            this.onRemoveModel(value.slice(1));
        });
        

       



    }
    private permissions = [];
    private memberPermissions = [];
    
    private permissionsProject = [];
    private permissionsIssue = [];
    private userData;
    get screenCommand() { return this.commandCtrl; }
    get screenChanged(): boolean {
        return this.ctrlMgr.changed == true;
    }


    // ------------- start date picker ------------------------
    private applyDate(value: any, dateInput: any) {
        dateInput.start = value.start;
        dateInput.end = value.end;
    }
    private selectedDate(value: any, dateInput: any) {
        dateInput.start = value.start;
        dateInput.end = value.end;
    }
    //-----------------end  date picker-------------------------- 

    private onDropModel(args) {
        let [el, target, source] = args;
        // do something else
    }
    
    private onRemoveModel(args) {
        let [el, source] = args;
        // do something else
    } 

    initScreen(): Promise<Object> {
        // console.log(localStorage);
        this.commandCtrl.useCustomCommand = true;
        this.commandCtrl.useEditCommand = true;
        this.commandCtrl.useUpdateCommand = true;
        this.commandCtrl.useCancelCommand = true;
        
        return this.loadData();
        
    }

    private loadData(): Promise<Object> {
        
        if(this.hasPermissionDeletePro()){
            this.commandCtrl.useDeleteCommand = true;
            this.commandCtrl.deleteCommandDisabled = false;
        }
        
        if(this.screenParam.newData){
            this.commandCtrl.useEditCommand = false;
            this.commandCtrl.cancelCommandDisabled = true;
            this.commandCtrl.updateCommandDisabled = false;
        }else{
            if(this.hasPermissionPJEdit()){
                this.commandCtrl.editCommandDisabled = false;
            }else{
                this.commandCtrl.editCommandDisabled = true;
            }
            this.commandCtrl.updateCommandDisabled = true;
            this.commandCtrl.cancelCommandDisabled = true;
        }
        
        let data = localStorage.getItem(USER_LOCAL_STORAGE);
        let usr = JSON.parse(data),Permissions,Issue,Project;
                            if(this.screenParam.newData){
                                this.ctrlMgr.setControlEnable(null, this.controlDisabled);
                            }else{
                                this.api.callApiController("api/PROJECT/GetProjectDetail", {
                                    type: "POST",
                                    data: {
                                        pj_id: usr.pj_id
                                    }
                                }).then(
                                    (result) => {
                                    this.ctrlMgr.data = result;
                                    this.mainInput.start = this.ctrlMgr.data.pj_start_date;
                                    this.mainInput.end = this.ctrlMgr.data.pj_end_date; 
                                });
                                this.api.callApiController("api/PROJECT/GetProjectMemberPermission", {
                                    type: "POST",
                                    data: {
                                        pj_id: usr.pj_id
                                    }
                                }).then(
                                    (result) => {
                                        Permissions = result;
                                        this.memberPermissions = Permissions;
                                });
                                
                            }
                            this.api.callApiController("api/PROJECT/GetProjectPermission", {
                                type: "POST"
                            }).then(
                                (result) => {
                                    Project = result;
                                    this.permissionsProject = Project;
                            });
                            this.api.callApiController("api/PROJECT/GetIssuePermission", {
                                type: "POST"
                            }).then(
                                (result) => {
                                    Issue = result;
                                    this.permissionsIssue = Issue;
                            });
                            this.api.callApiController("api/PROJECT/GetPhaseOption", {
                                type: "POST",
                                data: {
                                    pj_id: usr.pj_id
                                }
                            }).then(
                                (result) => {
                                this.PhaseOption = result;
                            });
                            this.api.callApiController("api/PROJECT/GetPhaseSelected", {
                                type: "POST",
                                data: {
                                    pj_id: usr.pj_id
                                }
                            }).then(
                                (result) => {
                                this.PhaseSelected = result;
                            });
                            this.api.callApiController("api/PROJECT/GetUserAll", {
                                type: "POST"
                            }).then(
                                (result) => {
                                this.UserAll = result;
                            });

        this.controlDisabled = !this.screenParam.newData;                        
        this.ctrlMgr.setControlEnable(null, !this.controlDisabled);
        this.MemberCtrlMgr.setControlEnable(null, !this.controlDisabled);

        return;
    }
    
    resetChanged() {
        this.ctrlMgr.resetChanged();
    }

    public confirmChange(): CONFIRM_CHANGE_SCREEN {
        return this.screenChanged ? CONFIRM_CHANGE_SCREEN.CONFIRM_UNSAVE : CONFIRM_CHANGE_SCREEN.CHANGE;
    }
    public confirmChangeResult(result: boolean) {

    }
    onUpdateCommand() {
        if(this.ctrlMgr.validate()){
            let data = localStorage.getItem(USER_LOCAL_STORAGE)
                ,usr = JSON.parse(data)
                ,pj;

            if(this.screenParam.newData){
                this.api.callApiController("api/PROJECT/InsertProject", {
                                        type: "POST",
                                        data: {
                                            pj_project_code : this.ctrlMgr.data.pj_project_code,
                                            pj_name         : this.ctrlMgr.data.pj_name,
                                            pj_description  : this.ctrlMgr.data.pj_description,
                                            us_id           : this.ctrlMgr.data.us_id,
                                            pjs_id          : this.ctrlMgr.data.pjs_id,
                                            pj_start_date   : this.mainInput.start,
                                            pj_end_date     : this.ctrlMgr.data.pj_end_date
                                        }
                                    }).then(
                                        (result) => {
                                        pj = result;
                                        usr.pj_id = pj.pj_id;
                                        localStorage.removeItem(USER_LOCAL_STORAGE);
                                        localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(usr));
                                        this.screenParam.newData = false;
                                        
                                    });
            }else{
                this.api.callApiController("api/PROJECT/UpdateProject", {
                    type: "POST",
                    data: {
                        pj_id           : usr.pj_id,
                        pj_project_code : this.ctrlMgr.data.pj_project_code,
                        pj_name         : this.ctrlMgr.data.pj_name,
                        pj_description  : this.ctrlMgr.data.pj_description,
                        us_id           : this.ctrlMgr.data.us_id,
                        pjs_id          : this.ctrlMgr.data.pjs_id,
                        pj_end_date     : this.ctrlMgr.data.pj_end_date
                    }
                });
            }
            setTimeout(()=>{
                if(usr.pj_id != 0){
                    this.Update();
                }else{
                    this.message.openMessageDialog("Project was exist. Use another data.",MESSAGE_DIALOG_TYPE.WARNING)
                }
            },750);
        }
    }
    private Update() {
        let data = localStorage.getItem(USER_LOCAL_STORAGE);
        let usr = JSON.parse(data)
            ,pjm
            ,Tmp = this.ctrlMgr.data
            ,member_per = this.ctrlMgr.data;

        Tmp.pp_pj_id = Tmp.pjm_pj_id = usr.pj_id
        Tmp.Member = this.memberPermissions;

        Tmp.Phase = [];
        member_per.Permissions = [];

        for(let i = 0;i < this.PhaseSelected.length;i++){
            Tmp.Phase.push({
                pp_pj_id: usr.pj_id,
                pp_ph_id: this.PhaseSelected[i].ph_id
            });
        }
        this.api.callApiController("api/PROJECT/UpdateProjectPhase", {
            type: "POST",
            data: Tmp
        });
        this.api.callApiController("api/PROJECT/UpdateProjectMember", {
            type: "POST",
            data: Tmp
        }).then(
        (result) => {
            pjm = result;
            for(let i = 0 ; i < this.memberPermissions.length ; i++){
                for(let j = 0 ; j < pjm.length ; j++){
                    if(pjm[j].pjm_us_id == this.memberPermissions[i].pjm_us_id){
                        if(this.memberPermissions[i]._editProject){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 1,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._editIssue){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 2,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._plan){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 3,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._processFix){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 4,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._check){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 5,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._close){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 6,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._miss){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 7,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._cancel){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 8,pmp_create_date: moment().format("YYYY-MM-DD")})}
                        if(this.memberPermissions[i]._reDebug){member_per.Permissions.push({pmp_pjm_id: pjm[j].pjm_id,pmp_pr_id: 9,pmp_create_date: moment().format("YYYY-MM-DD")})}
                    }
                }
            }
            this.api.callApiController("api/PROJECT/UpdateProjectMemberPermission", {
                type: "POST",
                data: member_per
            });
        });
        setTimeout(()=>{
            location.reload();
        },750);
    }
    onCancelCommand(){
        this.loadData();
    }
    onEditCommand() {
        // this.userData.totalColumns = 3;
        this.controlDisabled = false;
        this.commandCtrl.editCommandDisabled = true;
        this.commandCtrl.updateCommandDisabled = false;
        this.commandCtrl.cancelCommandDisabled = false;
        this.ctrlMgr.setControlEnable(null, !this.controlDisabled);
        this.MemberCtrlMgr.setControlEnable(null, !this.controlDisabled);
    }
    onDeleteCommand() { 
        let data = localStorage.getItem(USER_LOCAL_STORAGE);
        let usr = JSON.parse(data)
        this.api.callApiController("api/PROJECT/DeleteProject", {
            type: "POST",
            data: usr
        });
        
        setTimeout(()=>{
            this.router.navigate(["/s/project/p"]);
        },750)
    }
    private onAddMember() {
        let data_mp = this.memberPermissions;
        let data_u = this.UserAll;
        if(this.MemberCtrlMgr.data.us_id == this.ctrlMgr.data.us_id){
            this.message.openMessageDialog("  This user is project owner.",MESSAGE_DIALOG_TYPE.WARNING)
            return;
        }
        for(let i = 0 ; i < data_mp.length ; i++){
            if(data_mp[i].pjm_us_id == this.MemberCtrlMgr.data.us_id){
                this.message.openMessageDialog("  This user is already exists.",MESSAGE_DIALOG_TYPE.WARNING)
                return;
            }
        }
        for(let i = 0 ; i < data_u.length ; i++){
            if(data_u[i].pjm_us_id == this.MemberCtrlMgr.data.us_id){
                data_u[i].number = (data_mp.length+1);
                data_u[i].pjm_start_data = moment().format("YYYY-MM-DDTHH:mm:ss");
                data_u[i].pjm_end_data = (moment().add('1','day').format("YYYY-MM-DDTHH:mm:ss") );
                data_mp.push(data_u[i]);
                return;
            }
        }
    }
    private onDeleteMember(row) {
        if(row == undefined) row = this.ctrlMgr.data.us_id;
        let rowNumber = 1;
        let nrows = [];
        for(let rIdx = 0; rIdx < this.memberPermissions.length; rIdx++) {
            if (this.memberPermissions[rIdx].pjm_us_id != row) {
                let nrow = this.memberPermissions[rIdx];
                nrow["number"] = rowNumber;
                nrows.push(nrow);
                rowNumber++
            }
        }
        this.memberPermissions = nrows;
    }
}