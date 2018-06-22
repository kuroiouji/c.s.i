import {
  Component,
  Input,
  Output,
  EventEmitter
}                           from "@angular/core";

import "style-loader!./issuedetail.component.scss";

@Component({
  selector: "issue-detail-criteria",
  templateUrl: "issuedetail.component.html"
})
export class IssueDetailCriteria {
    public history:any;
    public member:any;
    
    constructor(
    ) { 
    }

    @Input()
    set historyRaw(row: any) {
      this.history = row;
    }
    @Input()
    set memberRaw(row: any) {
        this.member = row;
      }
    
    private getMemberByStatus(status:string){
        let obj=[];
        if(this.member != undefined){
            $.each(this.member,function(i,val){
                if(val.status == status){
                    obj.push(val);
                }
            });
        }
        return obj;
    }

}