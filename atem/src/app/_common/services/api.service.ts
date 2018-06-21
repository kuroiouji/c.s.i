import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";

import { USER_LOCAL_STORAGE, MESSAGE_DIALOG_TYPE, MESSAGE_DIALOG_RETURN_TYPE } from "../common.constant";
import { textFormat } from "../functions";
import { CONSTANT, CtmConstantService } from "../../_custom";
import { CtmApiLoader } from "./api.loader.service";
import { CtmMessageService } from "./message.service";
import { CtmTranslateService } from "./translate.service";

let moment = require("moment");

@Injectable()
export class CtmApiService {
    constructor(
        private http: HttpClient,
        private router: Router,
        private apiLoader: CtmApiLoader,
        private translate: CtmTranslateService,
        private message: CtmMessageService,
        private constant: CtmConstantService
    ) {

    }

    public callApiController(url: string, option: any = null): Promise<Object> {
        let iopt = {
			userKey: USER_LOCAL_STORAGE,
			anonymous: false,
			loginUrl: "/login",
			error503Url: "/503",
			loadingDiv: null,
			showLoading: true,
			type: "GET",
			data: { },
			showError: true,
			url: url,
			result: false,

			errorHandler: (code, params, data) => {
				return {
					type: MESSAGE_DIALOG_TYPE.ERROR,
					code: code,
					params: params
				};
			}
        };
        if (option != undefined) {
            for(let prop in option) {
                if (option[prop] != undefined) {
                    iopt[prop] = option[prop];
                }
            }
        }
        
        return this.constant.initialConstant()
            .then(() => {
                return this.checkToken(iopt)
                        .then((opt) => {
                            if (opt != undefined) {
                                if (opt["result"] != true) {
                                    if (opt["loginUrl"] != undefined) {
                                        this.router.navigate([opt["loginUrl"]]);
                                    }
                                    return false;
                                }

                                let token;
                                let loct = localStorage.getItem(opt["userKey"] + ".TOKEN");
                                if (loct != undefined) {
                                    token = loct;
                                }

                                let headers = new HttpHeaders({
                                    "Content-Type": "application/json; charset=UTF-8"
                                });
                                if (token != undefined) {
                                    headers = new HttpHeaders({
                                        "Content-Type": "application/json; charset=UTF-8",
                                        "Authorization": "Bearer " + token
                                    });
                                }
                                
                                if (opt["showLoading"] == true) {
                                    this.apiLoader.show(opt["loadingDiv"]);
                                }

                                if (opt["type"] == "POST") {
                                    let odata = JSON.stringify(opt["data"]);
                                    return this.http.post(CONSTANT.SERVER_PATH + url, odata, { "headers": headers })
                                        .toPromise()
                                        .then((res) => {
                                            return this.extractData(res, opt);
                                        })
                                        .catch((error) => {
                                            if (opt["showError"] == true) {
                                                return this.handleError(error, opt);
                                            }
                                            else {
                                                if (opt["showLoading"] == true) {
                                                    this.apiLoader.hide(500, opt["loadingDiv"]);
                                                }
                                                return null;
                                            }
                                        });
                                }
                                else {
                                    return this.http.get(CONSTANT.SERVER_PATH + url, { "headers": headers })
                                        .toPromise()
                                        .then((res) => {
                                            return this.extractData(res, opt);
                                        })
                                        .catch((error) => {
                                            if (opt["showError"] == true) {
                                                return this.handleError(error, opt);
                                            }
                                            else {
                                                if (opt["showLoading"] == true) {
                                                    this.apiLoader.hide(500, opt["loadingDiv"]);
                                                }
                                                return null;
                                            }
                                        });
                                }
                            }
                        });
            });
    }

    public checkToken(opt) {
        return new Promise((resolve, reject) => {
            if (opt.anonymous) {
                opt.result = true;
                resolve(opt);
                return;	
            }

            let usr;
            let token;
            let rtoken;
            let loc = localStorage.getItem(opt.userKey);
            if (loc != undefined) {
                usr = JSON.parse(loc);
            }
            
            if (usr != undefined) {
                if (usr.timeout != undefined && usr.date != undefined) {
                    var date = moment(usr.date);
                    var now = moment(new Date());
                    var duration = moment.duration(now.diff(date));
                    var minutes = duration.asMinutes();

                    if (usr.processing) {
                        if (minutes > usr.timeout) {
                            opt.result = false;
                            resolve(opt);
                            return;
                        }
                        else {
                            setTimeout(() => {
                                this.checkToken(opt)
                                    .then((res) => {
                                        resolve(res);
                                    });
                            }, 1000);
                            return;
                        }
                    }

                    if (minutes > ((3 * usr.timeout)/4)) {
                        usr.processing = true;
                        localStorage.removeItem(opt.userKey);
                        localStorage.setItem(opt.userKey, JSON.stringify(usr));

                        let loct = localStorage.getItem(opt.userKey + ".TOKEN");
                        if (loct != undefined) {
                            token = loct;
                            
                            loct = localStorage.getItem(opt.userKey + ".RTOKEN");
                            if (loct != undefined) {
                                rtoken = loct;
                            }
                        }

                        if (rtoken == undefined) {
                            opt.result = false;
                            resolve(opt);
                        }
                        else {
                            let headers = new HttpHeaders({
                                "Content-Type": "application/json; charset=UTF-8"
                            });
                            if (token != undefined) {
                                headers = new HttpHeaders({
                                    "Content-Type": "application/json; charset=UTF-8",
                                    "Authorization": "Bearer " + token
                                });
                            }

                            this.http.post(CONSTANT.SERVER_PATH + "api/Common/RefreshToken", 
                                    { Value: rtoken }, { "headers": headers })
                                .toPromise()
                                .then((res) => {
                                    if (res["Data"] != undefined) {
                                        if (res["Data"]["Data"] == true) {
                                            opt.result = true;
                                            resolve(opt);
                                        }
                                        else if (res["Data"]["Data"] != undefined) {
                                            let loc = {
                                                userName: res["Data"]["Data"]["UserName"],
                                                displayName: res["Data"]["Data"]["DisplayName"],
                                                groupID:  res["Data"]["Data"]["GroupID"],
                                                timeout: res["Data"]["Data"]["Timeout"],
                                                date: new Date()
                                            };
                                            localStorage.removeItem(opt.userKey);
                                            localStorage.setItem(opt.userKey, JSON.stringify(loc));
                
                                            localStorage.removeItem(opt.userKey + ".TOKEN");
                                            localStorage.setItem(opt.userKey + ".TOKEN", res["Data"]["Data"]["Token"]);

                                            localStorage.removeItem(opt.userKey + ".RTOKEN");
                                            localStorage.setItem(opt.userKey + ".RTOKEN", res["Data"]["Data"]["RefreshToken"]);

                                            opt.result = true;
                                            resolve(opt);
                                        }
                                        else {
                                            opt.result = false;
                                            resolve(opt);
                                        }
                                    }
                                    else {
                                        opt.result = false;
                                        resolve(opt);
                                    }
                                }).catch((error) => {
                                    if (error != undefined) {
                                        if (error.status == 0) {
                                            reject();
                                        }
                                        else {
                                            opt.result = false;
                                            resolve(opt);
                                        }
                                    }
                                    else {
                                        opt.result = false;
                                        resolve(opt);
                                    }
                                });
                        }
                    }
                    else {
                        usr.date = new Date();
                        localStorage.removeItem(opt.userKey);
                        localStorage.setItem(opt.userKey, JSON.stringify(usr));
                        
                        opt.result = true;
                        resolve(opt);
                    }
                }
            }
            else {
                opt.result = false;
                resolve(opt);
            }
        });
    }

    private extractData(res, opt) {
		if (res != undefined) {
			if (res["IsSystemError"] == true) {
				if (opt["showError"] == true) {
					if (this.apiLoader.isShow(opt["loadingDiv"]) == true) {
						this.apiLoader.hide(0, opt["loadingDiv"]);
					}
					return this.message.openMessageDialog(
                            this.translate.instant("CLE002", "MESSAGE"), 
                            MESSAGE_DIALOG_TYPE.ERROR)
                            .then(() => { return null; });
				}

				return null;
			}
			else if (res["Data"] != undefined) {
				if (res["Data"]["Errors"] != undefined) {
					if (res["Data"]["Errors"].length > 0) {
						return this.showErrorMessage(res, 0, opt);
					}
				}

				return res["Data"]["Data"];
			}
		}

        if (this.apiLoader.isShow(opt["loadingDiv"]) == true) {
            this.apiLoader.hide(500, opt["loadingDiv"]);
        }
		return null;
    }
    private showErrorMessage(res, idx, opt) {
        if (opt["showError"] == true 
            && res["Data"]["Errors"] != undefined) {
			if (idx < res["Data"]["Errors"].length) {
				let sperrs = res["Data"]["Errors"][idx].split(";");
				let code = sperrs[0];
				let params = [];
				if (sperrs.length > 1) {
					for(let i = 1; i < sperrs.length; i++) {
						params.push(sperrs[i]);
					}
				}

				let show = false;
				if (this.apiLoader.isShow(opt["loadingDiv"]) == true) {
					this.apiLoader.hide(0, opt["loadingDiv"]);
					show = true;
				}

				let msg = opt["errorHandler"](code, params, res["Data"]["Data"]);
				return this.message.openMessageDialog(
					textFormat(this.translate.instant(msg.code, "MESSAGE"), msg.params),
					msg.type)
					.then((type: MESSAGE_DIALOG_RETURN_TYPE) => {
						if (msg.MessageType == MESSAGE_DIALOG_TYPE.QUESTION
							|| msg.MessageType == MESSAGE_DIALOG_TYPE.WARNING_QUESTION
							|| msg.MessageType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL 
							|| msg.MessageType == MESSAGE_DIALOG_TYPE.QUESTION_OK) {

							if (type == MESSAGE_DIALOG_RETURN_TYPE.OK
								|| type == MESSAGE_DIALOG_RETURN_TYPE.YES) {
                                if (show) {
                                    this.apiLoader.show(opt["loadingDiv"]);
                                }
                                return this.showErrorMessage(res, idx + 1, opt);
							}
							else {
								return null;
							}
						}
						else {
                            if (show) {
                                this.apiLoader.show(opt["loadingDiv"]);
                            }
							return this.showErrorMessage(res, idx + 1, opt);
						}
					});
			}
			else {
                if (this.apiLoader.isShow(opt["loadingDiv"]) == true) {
                    this.apiLoader.hide(500, opt["loadingDiv"]);
                }
				return res["Data"]["Data"]? res["Data"]["Data"]: null;
			}
		}
		else {
            if (this.apiLoader.isShow(opt["loadingDiv"]) == true) {
                this.apiLoader.hide(500, opt["loadingDiv"]);
            }
			return res["Data"]["Data"]? res["Data"]["Data"] : null;
		}
    }
    private handleError(error, opt) {
		// In a real world app, we might use a remote logging infrastructure
		if (error instanceof Response) {
			if (error.status == 0) {
				if (opt["error503Url"] != "") {
					this.router.navigate([opt["error503Url"]]);
				}
			}
			else if (error.status == 401
				|| error.status == 404) {
				this.message.openMessageDialog(
					this.translate.instant("CLE001", "MESSAGE"))
					.then(() => {
                        if (opt["loginUrl"] != undefined) {
                            this.router.navigate([opt["loginUrl"]]);
                        }
					});
			}
			else {
				this.message.openMessageDialog(
					this.translate.instant("CLE002", "MESSAGE"));
			}
		}

		return null;
	}
}