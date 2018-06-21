(window["webpackJsonp_name_"] = window["webpackJsonp_name_"] || []).push([[3],{

/***/ "./node_modules/css-loader/index.js!./node_modules/sass-loader/lib/loader.js!./src/app/screens/security/+login/login.component.scss":
/*!*********************************************************************************************************************************!*\
  !*** ./node_modules/css-loader!./node_modules/sass-loader/lib/loader.js!./src/app/screens/security/+login/login.component.scss ***!
  \*********************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("exports = module.exports = __webpack_require__(/*! ../../../../../node_modules/css-loader/lib/css-base.js */ \"./node_modules/css-loader/lib/css-base.js\")(false);\n// imports\n\n\n// module\nexports.push([module.i, \".grid {\\n  display: grid;\\n  grid-template-columns: repeat(8, 1fr);\\n  grid-gap: 6px; }\\n  .grid .btn-login {\\n    grid-row-start: 2; }\\n\", \"\"]);\n\n// exports\n\n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.component.scss?./node_modules/css-loader!./node_modules/sass-loader/lib/loader.js");

/***/ }),

/***/ "./src/app/screens/security/+login/login.component.html":
/*!**************************************************************!*\
  !*** ./src/app/screens/security/+login/login.component.html ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("module.exports = \"<div class=\\\"grid\\\" [formGroup]=\\\"ctrlMgr.form\\\">\\n  <ctm-textbox subject=\\\"Username\\\" \\n              formControlName=\\\"Username\\\"\\n              [error]=\\\"ctrlMgr.error('Username')\\\"></ctm-textbox>\\n  <ctm-textbox type=\\\"password\\\" subject=\\\"Password\\\"\\n              formControlName=\\\"Password\\\"\\n              [error]=\\\"ctrlMgr.error('Password')\\\"></ctm-textbox>\\n  <button class=\\\"btn btn-login\\\" (click)=\\\"onLogin()\\\">Login</button>\\n</div>\"\n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.component.html?");

/***/ }),

/***/ "./src/app/screens/security/+login/login.component.scss":
/*!**************************************************************!*\
  !*** ./src/app/screens/security/+login/login.component.scss ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("\n        var result = __webpack_require__(/*! !../../../../../node_modules/css-loader!../../../../../node_modules/sass-loader/lib/loader.js!./login.component.scss */ \"./node_modules/css-loader/index.js!./node_modules/sass-loader/lib/loader.js!./src/app/screens/security/+login/login.component.scss\");\n\n        if (typeof result === \"string\") {\n            module.exports = result;\n        } else {\n            module.exports = result.toString();\n        }\n    \n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.component.scss?");

/***/ }),

/***/ "./src/app/screens/security/+login/login.component.ts":
/*!************************************************************!*\
  !*** ./src/app/screens/security/+login/login.component.ts ***!
  \************************************************************/
/*! exports provided: LoginComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"LoginComponent\", function() { return LoginComponent; });\n/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ \"./node_modules/tslib/tslib.es6.js\");\n/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ \"./node_modules/@angular/core/fesm5/core.js\");\n/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ \"./node_modules/@angular/router/fesm5/router.js\");\n/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ \"./node_modules/@angular/forms/fesm5/forms.js\");\n/* harmony import */ var _common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../../_common */ \"./src/app/_common/index.ts\");\n/* harmony import */ var _custom__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../../_custom */ \"./src/app/_custom/index.ts\");\n\r\n\r\n\r\n\r\n\r\n\r\nvar LoginComponent = /** @class */ (function () {\r\n    function LoginComponent(route, router, fb, translate, message, api) {\r\n        this.route = route;\r\n        this.router = router;\r\n        this.fb = fb;\r\n        this.translate = translate;\r\n        this.message = message;\r\n        this.api = api;\r\n        this.ctrlMgr = new _common__WEBPACK_IMPORTED_MODULE_4__[\"CtmControlManagement\"](fb.group({\r\n            \"Username\": [{ value: null, disabled: false }, _angular_forms__WEBPACK_IMPORTED_MODULE_3__[\"Validators\"].compose([_angular_forms__WEBPACK_IMPORTED_MODULE_3__[\"Validators\"].required])],\r\n            \"Password\": [{ value: null, disabled: false }]\r\n        }));\r\n    }\r\n    LoginComponent.prototype.ngOnInit = function () {\r\n        this.translate.use(_custom__WEBPACK_IMPORTED_MODULE_5__[\"CONSTANT\"].DEFAULT_LANGUAGE);\r\n        // get return url from route parameters or default to '/'\r\n        this.returnUrl = this.route.snapshot.queryParams[\"returnUrl\"] || \"/\";\r\n    };\r\n    LoginComponent.prototype.ngAfterViewInit = function () {\r\n        this.api.callApiController(\"api/CMS010/Initial\", {\r\n            type: \"POST\",\r\n            showLoading: false,\r\n            showError: false,\r\n            anonymous: true\r\n        }).then(function () {\r\n            //this.apiLoader.hide(500);\r\n            //this.userName.setFocus();\r\n        });\r\n    };\r\n    LoginComponent.prototype.onLogin = function () {\r\n        var _this = this;\r\n        if (this.ctrlMgr.validate(function (field, key) {\r\n            if (field == \"password\"\r\n                && (key == \"minlength\" || key == \"maxlength\")) {\r\n                return _this.translate.instant(\"CLE002\", \"MESSAGE\");\r\n            }\r\n            return null;\r\n        })) {\r\n            var data = this.ctrlMgr.data;\r\n            this.api.callApiController(\"api/CMS010/Login\", {\r\n                type: \"POST\",\r\n                anonymous: true,\r\n                data: data\r\n            }).then(function (res) {\r\n                if (res != undefined) {\r\n                    if (res[\"IsPasswordExpired\"] == true) {\r\n                    }\r\n                    else {\r\n                        var loc = {\r\n                            userName: res[\"UserName\"],\r\n                            displayName: res[\"DisplayName\"],\r\n                            groupID: res[\"GroupID\"],\r\n                            timeout: res[\"Timeout\"],\r\n                            date: new Date()\r\n                        };\r\n                        localStorage.removeItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"]);\r\n                        localStorage.setItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"], JSON.stringify(loc));\r\n                        localStorage.removeItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"] + \".TOKEN\");\r\n                        localStorage.setItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"] + \".TOKEN\", res[\"Token\"]);\r\n                        localStorage.removeItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"] + \".RTOKEN\");\r\n                        localStorage.setItem(_common__WEBPACK_IMPORTED_MODULE_4__[\"USER_LOCAL_STORAGE\"] + \".RTOKEN\", res[\"RefreshToken\"]);\r\n                        _this.router.navigate([_this.returnUrl]);\r\n                    }\r\n                }\r\n            });\r\n        }\r\n    };\r\n    LoginComponent = tslib__WEBPACK_IMPORTED_MODULE_0__[\"__decorate\"]([\r\n        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__[\"Component\"])({\r\n            selector: 'app-login',\r\n            template: __webpack_require__(/*! ./login.component.html */ \"./src/app/screens/security/+login/login.component.html\"),\r\n            styles: [__webpack_require__(/*! ./login.component.scss */ \"./src/app/screens/security/+login/login.component.scss\")]\r\n        }),\r\n        tslib__WEBPACK_IMPORTED_MODULE_0__[\"__metadata\"](\"design:paramtypes\", [_angular_router__WEBPACK_IMPORTED_MODULE_2__[\"ActivatedRoute\"],\r\n            _angular_router__WEBPACK_IMPORTED_MODULE_2__[\"Router\"],\r\n            _angular_forms__WEBPACK_IMPORTED_MODULE_3__[\"FormBuilder\"],\r\n            _common__WEBPACK_IMPORTED_MODULE_4__[\"CtmTranslateService\"],\r\n            _common__WEBPACK_IMPORTED_MODULE_4__[\"CtmMessageService\"],\r\n            _common__WEBPACK_IMPORTED_MODULE_4__[\"CtmApiService\"]])\r\n    ], LoginComponent);\r\n    return LoginComponent;\r\n}());\r\n\r\n\n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.component.ts?");

/***/ }),

/***/ "./src/app/screens/security/+login/login.module.ts":
/*!*********************************************************!*\
  !*** ./src/app/screens/security/+login/login.module.ts ***!
  \*********************************************************/
/*! exports provided: LoginModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"LoginModule\", function() { return LoginModule; });\n/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ \"./node_modules/tslib/tslib.es6.js\");\n/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ \"./node_modules/@angular/core/fesm5/core.js\");\n/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ \"./node_modules/@angular/common/fesm5/common.js\");\n/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ \"./node_modules/@angular/common/fesm5/http.js\");\n/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ \"./node_modules/@angular/forms/fesm5/forms.js\");\n/* harmony import */ var _common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../../_common */ \"./src/app/_common/index.ts\");\n/* harmony import */ var _login_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./login.component */ \"./src/app/screens/security/+login/login.component.ts\");\n/* harmony import */ var _login_routing__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./login.routing */ \"./src/app/screens/security/+login/login.routing.ts\");\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nvar LoginModule = /** @class */ (function () {\r\n    function LoginModule() {\r\n    }\r\n    LoginModule = tslib__WEBPACK_IMPORTED_MODULE_0__[\"__decorate\"]([\r\n        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__[\"NgModule\"])({\r\n            imports: [\r\n                _angular_common__WEBPACK_IMPORTED_MODULE_2__[\"CommonModule\"],\r\n                _angular_common_http__WEBPACK_IMPORTED_MODULE_3__[\"HttpClientModule\"],\r\n                _angular_forms__WEBPACK_IMPORTED_MODULE_4__[\"FormsModule\"],\r\n                _angular_forms__WEBPACK_IMPORTED_MODULE_4__[\"ReactiveFormsModule\"],\r\n                _login_routing__WEBPACK_IMPORTED_MODULE_7__[\"routing\"],\r\n                _common__WEBPACK_IMPORTED_MODULE_5__[\"CtmModule\"]\r\n            ],\r\n            declarations: [_login_component__WEBPACK_IMPORTED_MODULE_6__[\"LoginComponent\"]]\r\n        })\r\n    ], LoginModule);\r\n    return LoginModule;\r\n}());\r\n\r\n\n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.module.ts?");

/***/ }),

/***/ "./src/app/screens/security/+login/login.routing.ts":
/*!**********************************************************!*\
  !*** ./src/app/screens/security/+login/login.routing.ts ***!
  \**********************************************************/
/*! exports provided: routes, routing */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"routes\", function() { return routes; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"routing\", function() { return routing; });\n/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/router */ \"./node_modules/@angular/router/fesm5/router.js\");\n/* harmony import */ var _login_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./login.component */ \"./src/app/screens/security/+login/login.component.ts\");\n\r\n\r\nvar routes = [\r\n    {\r\n        path: \"\",\r\n        component: _login_component__WEBPACK_IMPORTED_MODULE_1__[\"LoginComponent\"]\r\n    }\r\n];\r\nvar routing = _angular_router__WEBPACK_IMPORTED_MODULE_0__[\"RouterModule\"].forChild(routes);\r\n\n\n//# sourceURL=webpack://%5Bname%5D/./src/app/screens/security/+login/login.routing.ts?");

/***/ })

}]);