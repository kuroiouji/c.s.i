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
    MESSAGE_DIALOG_TYPE,
    MESSAGE_DIALOG_RETURN_TYPE,
    ROLE_PERMISSION_TABLE    
}                                       from "../../common";

import "style-loader!./userinfo.component.scss";

@Component({
  selector: "user-info",
  templateUrl: "./userinfo.component.html"  
})
export class userInfoComponent extends ABaseComponent {
    
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
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

        //-----------------------highchart --------------------------------------------
        this.options = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Browser market shares January, 2015 to May, 2015'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true
                    },
                    showInLegend: false
                }
            },
            series: [{
                name: 'Brands',
                colorByPoint: true,
                data: [{
                    name: 'Microsoft Internet Explorer',
                    y: 56.33
                }, {
                    name: 'Chrome',
                    y: 24.03,
                    sliced: true,
                    selected: true
                }, {
                    name: 'Firefox',
                    y: 10.38
                }, {
                    name: 'Safari',
                    y: 4.77
                }, {
                    name: 'Opera',
                    y: 0.91
                }, {
                    name: 'Proprietary or Undetectable',
                    y: 0.2
                }]
            }]
        };
        //-------------------------------end highchart -----------------------------------
        
    }

    options: Object;

    initScreen(): Promise<Object> {
        
        return;
    }
    get screenCommand() { return this.commandCtrl; }
     screenChanged: boolean = false;
  resetChanged() { }
}
