export function tabToControl(event, scope = null) {
    let ctrls;
    if (scope != undefined) {
        ctrls = scope.find("input, button");
    }
    else {
        ctrls = $("my-app").find("input, button");
    }

    let cidx = ctrls.index($(event.target));
    if (event.shiftKey == true) {
        let idx = cidx - 1;
        if (idx >= 0) {
            while(cidx != idx) {
                if (idx < 0) {
                    idx = ctrls.length;
                }
                
                if ($(ctrls[idx]).attr("disabled")
                    || $(ctrls[idx]).hasClass("btn-autocomplete")) {
                    idx--;
                }
                else {
                    $(ctrls[idx]).focus();
                    break;
                }
            }
        }
    }
    else {
        let idx = cidx + 1;
        if (idx >= 0) {
            while(cidx != idx) {
                if (idx >= ctrls.length) {
                    idx = 0;
                }
                
                if ($(ctrls[idx]).attr("disabled")
                    || $(ctrls[idx]).hasClass("btn-autocomplete")) {
                    idx++;
                }
                else {
                    $(ctrls[idx]).focus();
                    break;
                }
            }
        }
    }
}