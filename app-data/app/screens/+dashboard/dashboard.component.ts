import { 
    Component, 
    ViewChild ,
    Input,
    Output,
    EventEmitter
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
      MESSAGE_DIALOG_TYPE,
      MESSAGE_DIALOG_RETURN_TYPE,
      ROLE_PERMISSION_TABLE,
      USER_LOCAL_STORAGE    
  }                                       from "../../common";
  
  import "style-loader!./dashboard.component.scss";
  
  @Component({
    selector: "dashboard",
    templateUrl: "./dashboard.component.html"  
  })
  export class DashboardComponent extends ABaseComponent {
          
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
    private project_data;
    
    private report_name:Array<string>=[];
    private report_open:Array<number>=[];
    private report_plan:Array<number>=[];
    private report_process:Array<number>=[];
    private report_fix:Array<number>=[];
    private report_check:Array<number>=[];
    private report_close:Array<number>=[];
    private report_re_debug:Array<number>=[];
    private report_miss:Array<number>=[];
    private report_cancel:Array<number>=[];
    private options: Object;
    
      constructor(
          router: Router,
          route: ActivatedRoute,
          spinner: BaThemeSpinner,
          translate: CtmTranslateService,
          message: CtmMessageService,
          command: CtmCommandService,
  
          private fb: FormBuilder,
          private api: ApiService,
          private table: CtmTableService
      ) {
          super(router, route, spinner, translate, message, command, {
           
              commandMgr: {
              }   
          });
          
          
      }
  
      

      initScreen(): Promise<Object> {
            this.loaddata();
          return;
      }
      warp(pj_id,pj_name,pj_project_code){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
            let usr = JSON.parse(loc);
            if (usr != undefined) {
                let data = {  
                    userName: usr.userName,
                    displayName: usr.displayName,
                    groupID:  usr.groupID,
                    pj_id: pj_id,
                    pj_name : pj_project_code+' ('+pj_name+')'
                };
                localStorage.removeItem(USER_LOCAL_STORAGE);
                localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(data));
            }
        }
        this.router.navigate(["/s/project/issue/"]);
      }


      public loaddata(){
        this.api.callApiController("api/MASTER/GetProjectDashBoard", {
            type: "POST",
            data: {
                us_username: this.getUserName()
            }
        }).then(
            (result) => {
                this.project_data = result;
            });

            this.api.callApiController("api/MASTER/GetProjectDashBoardReport", {
                type: "POST",
                data: {
                    us_username: this.getUserName()
                }
            }).then(
                (result) => {
                    if(result!=undefined){  
                        var data:any;
                        data = result;
                        for(let i=0;i<data.length;i++){
                            this.report_name.push(data[i].pj_name);
                            this.report_open.push(data[i].Open);
                            this.report_plan.push(data[i].Plan);
                            this.report_process.push(data[i].Process);
                            this.report_fix.push(data[i].Fix);
                            this.report_check.push(data[i].Check);
                            this.report_close.push(data[i].Close);
                            this.report_re_debug.push(data[i].Re_debug);
                            this.report_miss.push(data[i].Miss);
                            this.report_cancel.push(data[i].Cancel);
                        }
                    } 
                    // this.report_name = ['xxxx',"YYYY","ssss","pppp","aaaaa"];

                    //-----------------------highchart --------------------------------------------
          this.options = {
            chart: {
                type: 'column'
            },
            title: {
                text: 'ปริมาณ issue แต่ละ status'
            },
            xAxis: {
                categories: this.report_name
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Total Issue'
                }
            },
            tooltip: {
                pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                shared: true
            },
            plotOptions: {
                column: {
                    stacking: 'percent'
                }
            },
            series: [{
                name: 'Open',
                data: this.report_open
            }, {
                name: 'Plan',
                data: this.report_plan
            }, {
                name: 'Process',
                data: this.report_process
            }, {
                name: 'Fix',
                data: this.report_fix
            }, {
                name: 'Check',
                data: this.report_check
            }, {
                name: 'Close',
                data: this.report_close
            }, {
                name: 'Miss',
                data: this.report_miss
            }, {
                name: 'Cancel',
                data: this.report_cancel
            }, {
                name: 'Re-debug',
                data: this.report_re_debug
            }]
        };
          //-------------------------------end highchart -----------------------------------
                   
          
                });    

    }
    private getUserName(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); 
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          return usr.userName;
        }
    }
    get screenCommand() { return this.commandCtrl; }
    get screenChanged(): boolean {
        return null;
    }
        resetChanged() { }
    }

    