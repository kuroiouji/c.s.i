export const AUTOCOMPLETE_CACHE = {
    "GROUP": {
        url: "api/Common/GetUserGroupAutoComplete",
        display: "NameEN",
        value: "GroupID"
    },
    "ACTIVE_STATUS":{
        url: "/api/Common/GetConstantAutoComplete",
        data:{
            ConstantCode: "ACTIVE_STATUS"
        },
        display: "NameEN",
        value:"ConstantValue"
    }
};