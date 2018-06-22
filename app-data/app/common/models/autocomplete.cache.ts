import {
    convertBoolean,
    convertNumber,
    textFormat
}                                   from "../base";
import { 
    AutocompleteData,
    AutoCompleteServer,
    AAutoCompleteCriteria 
}                                   from "./autocomplete.data";
import { CtmTranslateService }      from "../services";

export class BranchCriteria 
                extends AAutoCompleteCriteria {
    public BrandCode: string;
    
    public get criteriaKey(): string {
        let key = "";
        if (this.BrandCode != undefined) {
            key = this.BrandCode;
        }

        return key;
    }
}
export class TheatreSubAutoCompleteCriteria 
                extends AAutoCompleteCriteria {
    public TheatreID: number;
    
    public get criteriaKey(): string {
        let key = "";
        if (this.TheatreID != undefined) {
            key = this.TheatreID.toString();
        }

        return key;
    }
}
export class TheatreSystemAutoCompleteCriteria 
                extends AAutoCompleteCriteria {
    public TheatreID: number;
    public TheatreSubID: number;

    public get criteriaKey(): string {
        let key = "";
        if (this.TheatreID != undefined) {
            key = this.TheatreID.toString();

            if (this.TheatreSubID != undefined) {
                key = ":" + this.TheatreSubID.toString();
            }
        }

        return key;
    }
}
export class PhaseAutoCompleteCriteria 
    extends AAutoCompleteCriteria {
        public pj_id: number;

        public get criteriaKey(): string {
        let key = "";
        if (this.pj_id != undefined) {
            key = this.pj_id.toString();
        }

        return key;
    }
}
export const AUTOCOMPLETE_CACHE = {
    "GROUP": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetUserGroupAutoComplete",
                    alwayLoad: true
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (translate.isLanguageEN == true) {
                        return data["NameEN"];
                    }
                    
                    return data["NameLC"];
                },
                "GroupID"),
    "USER": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetUserAutoComplete"
                }),
                "UserName",
                "UserName"),
    "ACTIVE_STATUS": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetConstantAutoComplete",
                    data: { ConstantCode: "ACTIVE_STATUS" }
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (translate.isLanguageEN == true) {
                        return data["NameEN"];
                    }
                    
                    return data["NameLC"];
                },
                "ConstantValue",
                function(val1, val2) {                    
                  return convertBoolean(val1) == convertBoolean(val2);
                },
                function (val) {
                    return convertBoolean(val);
                }),
    "DISPLAY_PAGE": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetConstantAutoComplete",
                    data: { ConstantCode: "DISPLAY_PAGE" }
                }),
                "NameEN",
                "ConstantValue",
                function(val1, val2) {
                  return convertNumber(val1) == convertNumber(val2);
                },
                function (val) {
                    return convertNumber(val);
                }),
    "GENDER": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetConstantAutoComplete",
                    data: { ConstantCode: "GENDER" }
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (translate.isLanguageEN == true) {
                        return data["NameEN"];
                    }
                    
                    return data["NameLC"];
                },
                "ConstantValue"),
    "SMALL_MASTER": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Master/GetSmallMasterAutoComplete"
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (translate.isLanguageEN == true) {
                        return data["MSTNameEN"];
                    }
                    
                    return data["MSTNameLC"];
                },
                "MSTCode"),
    "BRAND": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Master/GetSmallMasterAutoComplete",
                    data: {
                        MSTCode: "030"
                    }
                }),
                "Description",
                "ValueCode"),
    "BRANCH": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Master/GetBranchAutoComplete"
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (data["BrandCode"] != undefined) {
                        return textFormat("{0}: {1}", [data["BrandCode"], data["BranchName"]]);
                    }

                    return data["BranchName"];
                },
                "BranchID",
                function(val1, val2) {
                    return convertNumber(val1) == convertNumber(val2);
                },
                function (val) {
                    return convertNumber(val);
                }),
    "REPORT_DURATION": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Common/GetConstantAutoComplete",
                    data: { ConstantCode: "REPORT_DURATION" }
                }),
                (translate: CtmTranslateService, data)=> { 
                    if (translate.isLanguageEN == true) {
                        return data["NameEN"];
                    }
                    
                    return data["NameLC"];
                },
                "ConstantValue"),
    "Priority": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetPriorityAutocomplete"
                }),
                "pt_name",
                "pt_id"),
    "Serverity": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetServerityAutocomplete"
                }),
                "st_name",
                "st_id"),
    "IssueStatus": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetIssueStatusAutocomplete"
                }),
                "iss_name",
                "iss_id"),
    "ProjectAll": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/Project/GetProjectAutoComplete"
                }),
                "pj_name",
                "pj_id"),
    "Phase": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetPhaseAutocomplete"
                }),
                "ph_name",
                "ph_id"),
    "Project_Mem": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetProjectMemberAutocomplete"
                }),
                "FullName",
                "us_id"),
    "AllUser": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/MASTER/GetUserProjectAutoComplete"
                }),
                "FullName",
                "us_id"),
    "ProjectStatus": new AutocompleteData(
                new AutoCompleteServer({
                    url: "api/PROJECT/GetProjectStatusAutoComplete"
                }),
                "pjs_name",
                "pjs_id")
};