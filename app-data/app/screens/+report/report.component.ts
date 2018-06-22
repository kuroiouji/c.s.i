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
    ROLE_PERMISSION_TABLE,
    USER_LOCAL_STORAGE    
}                                       from "../../common";

import "style-loader!./report.component.scss";

@Component({
  selector: "report",
  templateUrl: "./report.component.html"  
})
export class ReportComponent extends ABaseComponent {
    
    @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
    private ctrlMgr: CtmControlManagement;
    private optionsData;
    private option2Data;
    private option3Data;
    private option4Data;
    private option5Data;

    private options: Object;
    private option2: object;
    private option3: object;
    private option4: object;
    private option5: object;


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
        this.ctrlMgr = new CtmControlManagement(
            fb.group({
                "pj_id": {value: null}
            })
        );

        
       
    }


    
    initScreen(): Promise<Object> {
        this.ctrlMgr.data = this.getUser();
        this.loaddata();
        return;
    }
    private ProjectChange(){
        this.loaddata();
    }
    private loaddata(){
        if(this.ctrlMgr.data.pj_id != undefined){
            this.api.callApiController("api/MASTER/GetProjectDashBoardReport", {
                type: "POST",
                data: {
                    pj_id : this.ctrlMgr.data.pj_id
                }
            }).then(
                (result) => {
                    this.optionsData = result[0];
                    this.options = {
                        chart: {
                            width: null,
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        title: {
                            text: 'Issue Status'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: 'Brands',
                            colorByPoint: true,
                            data: [{
                                name: 'Open',
                                y: this.optionsData.Open
                            }, {
                                name: 'Plan',
                                y: this.optionsData.Plan,
                                sliced: true,
                                selected: true
                            }, {
                                name: 'Process',
                                y: this.optionsData.Process
                            }, {
                                name: 'Fix',
                                y: this.optionsData.Fix
                            }, {
                                name: 'Close',
                                y: this.optionsData.Close
                            }, {
                                name: 'Re-debug',
                                y: this.optionsData.Re_debug
                            }, {
                                name: 'Miss',
                                y: this.optionsData.Miss
                            }, {
                                name: 'Cancel',
                                y: this.optionsData.Cancel
                            }]
                        }]
                    };
                });
                    this.api.callApiController("api/MASTER/GetReportPriority", {
                        type: "POST",
                        data: {
                            pj_id : this.ctrlMgr.data.pj_id
                        }
                    }).then(
                        (result) => {
                            this.option3Data = result;
                            let data = []
                            for(let i=0;i<this.option3Data.length;i++){
                                data.push({
                                    neme:this.option3Data[i].name,
                                    data:[
                                        this.option3Data[i].type1,
                                        this.option3Data[i].type2,
                                        this.option3Data[i].type3,
                                        this.option3Data[i].type4]
                                });
                            }
                            this.option3 = {
                                chart: {
                                    type: 'column'
                                },
                                title: {
                                    text: 'ระยะเวลาในการแก้ไข Issue โดย Priority'
                                },
                                xAxis: {
                                    categories: ['1 day', '< 1 week', '1-2 week', '> 2 week']
                                },
                                yAxis: {
                                    min: 0,
                                    title: {
                                        text: 'Total issue by priority'
                                    },
                                    stackLabels: {
                                        enabled: true,
                                        style: {
                                            fontWeight: 'bold',
                                            color: 'gray'
                                        }
                                    }
                                },
                                legend: {
                                    align: 'right',
                                    x: -30,
                                    verticalAlign: 'top',
                                    y: 25,
                                    floating: true,
                                    backgroundColor:'white',
                                    borderColor: '#CCC',
                                    borderWidth: 1,
                                    shadow: false
                                },
                                tooltip: {
                                    headerFormat: '<b>{point.x}</b><br/>',
                                    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                                },
                                plotOptions: {
                                    column: {
                                        stacking: 'normal',
                                        dataLabels: {
                                            enabled: true,
                                            color:  'white'
                                        }
                                    }
                                },                                
                                series: data
                                
                            };
                        });
       
        // --------------------option 3 ---------------------------------
        this.api.callApiController("api/MASTER/GetReportCausePhase", {
            type: "POST",
            data: {
                pj_id : this.ctrlMgr.data.pj_id
            }
        }).then(
            (result) => {
                this.option4Data = result;
                let data = []
                for(let i=0;i<this.option4Data.length;i++){
                    data.push([this.option4Data[i].ph_name,this.option4Data[i].number]);
                }
        this.option4 = {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Phase ที่เกิด Issue'
            },
            subtitle: {
                text: '..'
            },
            xAxis: {
                type: 'category',
                labels: {
                    rotation: -45,
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'จำนวน Issue'
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                pointFormat: 'จำนวน Issue : <b>{point.y:.0f}</b>'
            },
            series: [{
                name: 'Phase',
                data: data,
                dataLabels: {
                    enabled: true,
                    rotation: 0,
                    color: '#FFFFFF',
                    align: 'center',
                    format: '{point.y:.0f}', // one decimal
                    y: 10, // 10 pixels down from the top
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            }]
        };
    });
        this.api.callApiController("api/MASTER/GetReportServerity", {
            type: "POST",
            data: {
                pj_id : this.ctrlMgr.data.pj_id
            }
        }).then(
            (result) => {
                this.option5Data = result;
                let data = []
                for(let i=0;i<this.option5Data.length;i++){
                    data.push({
                        neme:this.option5Data[i].name,
                        data:[
                            this.option5Data[i].type1,
                            this.option5Data[i].type2,
                            this.option5Data[i].type3,
                            this.option5Data[i].type4]
                    });
                }
                this.option5 = {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'ระยะเวลาในการแก้ไข Issue โดย Serverity'
                    },
                    xAxis: {
                        categories: ['1 day', '< 1 week', '1-2 week', '> 2 week']
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Total issue by Serverity'
                        },
                        stackLabels: {
                            enabled: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'gray'
                            }
                        }
                    },
                    legend: {
                        align: 'right',
                        x: -30,
                        verticalAlign: 'top',
                        y: 25,
                        floating: true,
                        backgroundColor:'white',
                        borderColor: '#CCC',
                        borderWidth: 1,
                        shadow: false
                    },
                    tooltip: {
                        headerFormat: '<b>{point.x}</b><br/>',
                        pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal',
                            dataLabels: {
                                enabled: true,
                                color:  'white'
                            }
                        }
                    },                                
                    series: data
                    
                };
            });

        }
    }

    private getUser(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE); 
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          return usr;
        }
    }

    get screenCommand() { return this.commandCtrl; }
     screenChanged: boolean = false;
  resetChanged() { }
}
