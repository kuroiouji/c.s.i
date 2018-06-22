import { Injectable }                   from "@angular/core";
import { 
    Router, 
    CanActivateChild, 
    ActivatedRouteSnapshot, 
    RouterStateSnapshot 
}                                       from "@angular/router";
import { 
    Http, 
    Headers, 
    RequestOptions, 
    Response 
}                                       from "@angular/http";

import { 
    USER_LOCAL_STORAGE,
    SERVER_PATH,
    SERVER }                  from "../common.constant";
import { ROLE_PERMISSION }              from "../common.permission";
import {
    CtmTranslateService,
    CtmMessageService
}                                       from "../services";

export class RoleData {
    public permissions: Array<RoleScreenPermissionData>;

    constructor(permissions) {
        this.permissions = [];
        if (permissions == undefined) {
            return;
        }

        for (let ipx = 0; ipx < permissions.length; ipx++) {
            let p = permissions[ipx];
            let rsp = new RoleScreenPermissionData();
            rsp.screenID = p.screenID;

            rsp.permissions = [];
            if (p.permissions != undefined) {
                for (let i = 0; i < p.permissions.length; i++) {
                    rsp.permissions.push(new RolePermissionData(p.permissions[i]));
                }  
            }

            this.permissions.push(rsp);
        }
    } 

    public hasPermission(p: ROLE_PERMISSION): boolean {
        return $.grep(this.permissions, function (val) {
            return $.grep(val.permissions, function(sval) {
                    return sval.permissionID == p && sval.hasPermission == true;
                }).length > 0;
        }).length > 0;
    } 
    public hasScreenPermission(s: string, p: ROLE_PERMISSION): boolean {
        return $.grep(this.permissions, function (val) {
            if (val.screenID == s) {
                return $.grep(val.permissions, function(sval) {
                    return sval.permissionID == p && sval.hasPermission == true;
                }).length > 0;
            }

            return false;
        }).length > 0;
    }
}
export class RoleScreenPermissionData {
    public screenID: string;
    public permissions: Array<RolePermissionData>;
}
export class RolePermissionData {
    public permissionID: ROLE_PERMISSION;
    public hasPermission: boolean;

    constructor(permissionID: ROLE_PERMISSION) {
        this.permissionID = permissionID;
    }
}

//------------------------------------------permission role--------------------------------------------------------------
export class PjRoleData {
    public permissions: RoleProjectPermissionData;

    constructor(permissions) {
        this.permissions = null;
        if (permissions == undefined) {
            return;
        }

            let p = permissions;
            let rsp = new RoleProjectPermissionData();
            rsp.pj_id = this.currentPj();
            rsp.us_username = this.currentUserName();

            rsp.permissions = [];
            if (p.permissions != undefined) {
                for (let i = 0; i < p.permissions.length; i++) {
                    rsp.permissions.push(new RoleProjectData(p.permissions[i]));
                }  
            }

            this.permissions = rsp;
    } 

    public hasPermission(p: ROLE_PERMISSION): boolean {
            return $.grep(this.permissions.permissions, function(sval) {
                    return sval.pr_id == p && sval.hasPermission == true;
                }).length > 0;
    } 
    public hasScreenPermission(s: string, p: ROLE_PERMISSION): boolean {
            if (this.permissions.pj_id == s) {
                return $.grep(this.permissions.permissions, function(sval) {
                    return sval.pr_id == p && sval.hasPermission == true;
                }).length > 0;
            }

            return false;
    }
    private currentUserName() {
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          if (usr != undefined) {
            return usr.userName;
          }
        }
  
        return "";
    }
    private currentPj(){
        let loc = localStorage.getItem(USER_LOCAL_STORAGE);
        if (loc != undefined) {
          let usr = JSON.parse(loc);
          if (usr != undefined) {
            return usr.pj_id;
          }
        }
  
        return "";
    }
}

export class RoleProjectPermissionData {
    public pj_id: any;
    public us_username: string;
    public permissions: Array<RoleProjectData>;
}
export class RoleProjectData {
    public pr_id: ROLE_PERMISSION;
    public hasPermission: boolean;

    constructor(pr_id: ROLE_PERMISSION) {
        this.pr_id = pr_id;
    }
}
//--------------------------------------PjRoleData--------------------------------------
@Injectable()
export class RoleGuard implements CanActivateChild {
    constructor(
        private http: Http,
        private router: Router,

        private message: CtmMessageService,
        private translate: CtmTranslateService        
    ) {
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let role = route.data["role"] as RoleData;
        let ProjectRole = route.data["ProjectRole"] as PjRoleData;
        
        let headers = new Headers({
            "Content-Type": "application/json; charset=UTF-8"
        });
        let options = new RequestOptions({
            "headers": headers
        });
        if (role == undefined && ProjectRole == undefined) {
            return true;
        }else if (role != undefined){        
            return this.http.post(SERVER.PATH + "api/Common/IsUserInRole",
            JSON.stringify(role), options)
            .toPromise()
            .then((res: Response) => {
                let result = res.json();
                if (result.Data == undefined) {
                    this.message.openMessageDialog(
                        this.translate.instant("CLE003", "MESSAGE"))
                        .then(() => {
                            this.router.navigate(["/login"]);
                        });
                        
                    return false;
                }

                let srole = route.data["role"] as RoleData;                
                for (let i = 0; i < srole.permissions.length; i++) {
                    let ps = result.Data["permissions"];
                    if (ps != undefined) {
                        for (let j = 0; j < srole.permissions[i].permissions.length; j++) {
                            srole.permissions[i].permissions[j].hasPermission = ps[i]["permissions"][j]["hasPermission"];
                        } 
                    }
                }

                if (srole.permissions[0].screenID == "CMS020") {
                    return true;
                }
                
                if (srole.hasScreenPermission(srole.permissions[0].screenID, ROLE_PERMISSION.OPEN) == false) {
                    if (srole.permissions[0].screenID == "CMS020") {
                        this.router.navigate(["/login"]);
                    }
                    else {
                        this.message.openMessageDialog(
                            this.translate.instant("CLE003", "MESSAGE"))
                            .then(() => {
                                this.router.navigate(["/"]);
                            });
                    }

                    return false;
                }

                return true;
            })
            .catch((error: Response) => {
                this.message.openMessageDialog(
                    this.translate.instant("CLE003", "MESSAGE"))
                    .then(() => {
                            this.router.navigate(["/login"]);
                        });
            });
        }//---------------------end else role -------------------//
        if(ProjectRole != undefined){
            return this.http.post(SERVER.PATH + "api/MASTER/CheckPermission",
            JSON.stringify(ProjectRole.permissions), options)
            .toPromise()
            .then((res: Response) => {
                let result = res.json();
                if (result.Data == undefined) {
                    this.message.openMessageDialog(
                        this.translate.instant("CLE003", "MESSAGE"))
                        .then(() => {
                            //this.router.navigate(["/login"]);
                        });
                        
                    return false;
                }
                
                let srole = route.data["ProjectRole"] as PjRoleData;    
                    let ps = result.Data["permissions"];
                    if (ps != undefined) {
                        for (let j = 0; j < srole.permissions.permissions.length; j++) {
                            srole.permissions.permissions[j].hasPermission = ps[j]["hasPermission"];
                       
                        } 
                    }
                return true;
            })
            .catch((error: Response) => {
                this.message.openMessageDialog(
                    this.translate.instant("CLE003", "MESSAGE"))
                    .then(() => {
                    //        this.router.navigate(["/login"]);
                        });
            });

        }
    
    }
}