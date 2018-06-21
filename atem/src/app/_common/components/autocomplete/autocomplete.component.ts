import { Component, OnInit, Input, Output, ViewEncapsulation, 
        forwardRef, ElementRef, EventEmitter, ViewChild,
        HostListener } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR  } from "@angular/forms";

import { cloneObject, tabToControl } from "../../functions";
import { ResponseMessage } from "../../models";
import { CtmApiService } from "../../services";
import { AUTOCOMPLETE_CACHE } from "../../../_custom";

export const CTM_AUTOCOMPLTE_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CtmAutocompleteComponent),
  multi: true
};

@Component({
  selector: 'ctm-autocomplete',
  templateUrl: './autocomplete.component.html',
  providers: [CTM_AUTOCOMPLTE_CONTROL_VALUE_ACCESSOR],
  host: {   
  },
  encapsulation: ViewEncapsulation.None
})
export class CtmAutocompleteComponent implements OnInit, ControlValueAccessor {
  @ViewChild("divAutocomplete") divAutocomplete;

  private inputValue: string = "";
  private selectedItem: any = null;
  private _value: any = "";
  private _readonly: boolean = false;
  private _error: ResponseMessage = new ResponseMessage();
  private isFocused: boolean = false;
  private showAutocomplete: boolean = false;
  private list: Array<Object> = [];
  private maxlist: number = 50;

  constructor(
    private element: ElementRef,
    private api: CtmApiService
  ) { 
  }

  ngOnInit() {
    $(".autocomplete-contents").append(this.divAutocomplete.nativeElement);
  }

  @HostListener("window:resize")
  public onWindowResize():void {
    if (this.showAutocomplete == true) {
      this.showAutocomplete = false;
    }
  }
  @HostListener("window:scroll")
  public onWindowScroll():void {
    if (this.showAutocomplete == true) {
      this.showAutocomplete = false;
    }
  }
  @HostListener("window:click", ['$event'])
  public onWindowClick(event):void {
    if (this.showAutocomplete == true) {
      if($(this.divAutocomplete.nativeElement).find($(event.target)).length == 0
          && $(this.element.nativeElement).find($(event.target)).length == 0) {
        this.showAutocomplete = false;
      }
    }
    
  }

  writeValue(value: any): void {
    if (value !== this._value) {
      this.inputValue = "";
      this._value = value;

      if (value != undefined) {
        this.filterData({
          value: value
        }).then(() => {
          if (this.list.length == 1) {
            let item = this.list[0];
            item["selected"] = true;
            this.inputValue = item["display"];
            this._value = item["value"];
            this.selectedItem = item["data"];
          }
        });
      }
    }
  }
  registerOnChange(fn: (value: any) => void): void { this._onChange = fn; }
  registerOnTouched(fn: () => {}): void { this._onTouched = fn; }
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  private _onChange = (value: any) => { };
  private _onTouched = () => { };

  @Input("subject")
  public subject: string;
  @Input("required")
  public required: boolean = false;
  @Input("type")
  public textType: string = "text";
  @Input()
  public tabindex: number = 0;
  @Input() 
  public placeholder: string = "SELECT";
  @Input("minlength") 
  public minLength: number = 0;
  @Input("maxlength")
  public maxLength: number = 100;
  @Input("tab-with-enter")
  public tabWithEnter: boolean = false;

  @Input()
  public get readonly(): boolean { 
    return this._readonly; 
  }
  public set readonly(value) { 
    this._readonly = value; 
  }

  @Input()
  public get disabled(): boolean { 
    return this._readonly;
  }
  public set disabled(value) { 
    this._readonly = value;
  }

  @Input()
  public get error(): ResponseMessage { 
      return this._error; 
  }
  public set error(value) {
    if (value != undefined) {
      if (typeof(value) == "string") {
        this._error = new ResponseMessage();
        this._error.type = "error";
        this._error.message = value;
      }
      else {
        this._error = value;
      } 
    }
  }

  @Input()
  public get value(): any { return this._value; }
  public set value(value: any)  {
    this._value = value;
    this.inputValue = "";
    if (value != undefined) {
      this.filterData({
        value: value
      }).then(() => {
        if (this.list.length == 1) {
          let item = this.list[0];
          item["selected"] = true;
          this.inputValue = item["display"];
          this._value = item["value"];
          this.selectedItem = item["data"];
        }
      });
    }

    this._onChange(value);
  }

  @Input()
  public key: string = null;
  @Input("default-value")
  public defaultValue: any = null;
  @Input("auto-select")
  public autoSelect: boolean = false;
  @Input("match-only")
  public matchOnly: boolean = true;
  @Input("custom-item")
  public customItem: Function;
  @Input("custom-filter")
  public customFilter: Function;

  @Output() 
  public change: EventEmitter<any> = new EventEmitter<any>();
  @Output("autocomplete-text-change") 
  public textChange = new EventEmitter();
  @Output() 
  public focus: EventEmitter<any> = new EventEmitter<any>();
  @Output() 
  public blur: EventEmitter<any> = new EventEmitter<any>();

  public setFocus() {
    this.isFocused = true;
    this.element.nativeElement.querySelector('input').focus();
  }

  private onKeydown(event) {
    if (this.disabled) {
      return;
    }

    switch (event.keyCode) {
      case 9:   //TAB
        event.stopPropagation();
        event.preventDefault();

        this.onSelectItemByInput();
        this.showAutocomplete = false;
        tabToControl(event);
        
        break;

      case 27:  //ESCAPE
        event.stopPropagation();
        event.preventDefault();
        
        break;

      case 13:  //ENTER
        event.preventDefault();
        event.stopPropagation();

        this.onSelectItemByInput();
        if (this.tabWithEnter) {
          tabToControl(event);
          return false;
        }
        else {
          setTimeout(() => {
            $(this.element.nativeElement).find("input").select();
          }, 0);
        }

        break;

      case 40:  //DOWN_ARROW
        event.preventDefault();
        event.stopPropagation();

        if (this.showAutocomplete == true) {
          if (this.list.length > 0) {
            let fIdx = -1;
            for(let idx = 0; idx < this.list.length; idx++) {
              if (this.list[idx]["selected"] == true) {
                this.list[idx]["selected"] = false;
                fIdx = idx;
                break;
              }
            }
            fIdx++;

            if (fIdx >= this.list.length) {
              fIdx = 0;
            }
            let item = this.list[fIdx];
            item["selected"] = true;
            this.inputValue = item["display"];
            this._value = item["value"];
            this.selectedItem = item["data"];
            this.updateScroll(fIdx);
          }
        }

        break;
      case 38:  //UP_ARROW
        event.preventDefault();
        event.stopPropagation();
        
        if (this.showAutocomplete == true) {
          if (this.list.length > 0) {
            let fIdx = this.list.length;
            for(let idx = 0; idx < this.list.length; idx++) {
              if (this.list[idx]["selected"] == true) {
                this.list[idx]["selected"] = false;
                fIdx = idx;
                break;
              }
            }
            fIdx--;

            if (fIdx < 0) {
              fIdx = this.list.length - 1;
            }
            let item = this.list[fIdx];
            item["selected"] = true;
            this.inputValue = item["display"];
            this._value = item["value"];
            this.selectedItem = item["data"];
            this.updateScroll(fIdx);
          }
        }

        break;
      default:
        setTimeout(() => {
          if (this.inputValue == "") {
            for(let i = 0; i < this.list.length; i++) {
              this.list[i]["selected"] = false;
            }
            
            this.inputValue = "";
            this.value = null;
            this.selectedItem = null;
            this.showAutocomplete = false;
          }
          else {
            this.filterData({
              display: this.inputValue
            });
            if (this.showAutocomplete == false) {
              this.generateAutocomplete();
            }
          }
        }, 0);
    }
  }
  private onKeyup(event) {
    if (this.disabled) { 
      return; 
    }

    this.textChange.emit({
        source: this,
        value: this.inputValue
    });
  }
  private onChange() {
    this._error.type = null;
    this._error.message = null;
  }
  private onFocus() {
    if (this._readonly == false) {
      this.isFocused = true;
      $(this.element.nativeElement).find("input").select();
    }
  }
  private onBlur() {
    this.isFocused = false;

    this._onTouched();
    this.blur.emit();
  }

  private get isRequired() {
    return this.disabled == false
            && this.required == true;
  }

  private get placeHolderText() {
    if (this.placeholder == "SELECT") {
      return "SELECT";
    }
    if (this.placeholder == "ALL") { 
      return "ALL";
    }
    else if (this.placeholder == "EMPTY") {
        return "";
    }
    
    return this.placeholder;
  }

  private onTrickerAutocomplete() {
    if (this.showAutocomplete == true) {
      this.showAutocomplete = false;
    }
    else {
      this.filterData({
        display: this.inputValue
      });
      this.generateAutocomplete();
    }
  }
  private onSelectItem(idx) {
    if (idx < this.list.length) {
      for(let i = 0; i < this.list.length; i++) {
        if (this.list[i]["selected"] == true) {
          this.list[i]["selected"] = false;
        }
      }

      let item = this.list[idx];
      item["selected"] = true;
      this.inputValue = item["display"];
      this._value = item["value"];
      this.selectedItem = item["data"];

      this._error.type = null;
      this._error.message = null;
      this._onChange(this._value);
      this.change.emit({
        source: this,
        value: this._value
      });

      this.showAutocomplete = false;
      $(this.element.nativeElement).find("input").focus();
      setTimeout(() => {
        $(this.element.nativeElement).find("input").select();
      }, 0);
    }
  }
  private onSelectItemByInput() {
    this.filterData({
      display: this.inputValue
    });
    if (this.inputValue != ""
        && this.list.length > 0) {
      for(let i = 0; i < this.list.length; i++) {
        if (this.list[i]["selected"] == true) {
          this.list[i]["selected"] = false;
        }
      }

      let item = this.list[0];
      item["selected"] = true;
      this.inputValue = item["display"];
      this._value = item["value"];
      this.selectedItem = item["data"];

      this._error.type = null;
      this._error.message = null;
      this._onChange(this._value);
      this.change.emit({
        source: this,
        value: this._value
      });
      
      this.showAutocomplete = false;
    }
    else {
      for(let i = 0; i < this.list.length; i++) {
        if (this.list[i]["selected"] == true) {
          this.list[i]["selected"] = false;
        }
      }

      this.inputValue = "";
      this.value = null;
      this.selectedItem = null;
    }
  }

  private generateAutocomplete() {
    this.showAutocomplete = true;
    setTimeout(() => {
      let a = $(this.divAutocomplete.nativeElement).find(".autocomplete");
          
      let ctrl = this.element.nativeElement.querySelector('.control');
      var top = 0, left = 0, c;
      c = ctrl;
      do {
          top += c.offsetTop || 0;
          left += c.offsetLeft || 0;
          
          c = c.offsetParent;
      } while(c);

      $(ctrl).parents().each(function() {
          if ($(this).hasClass("table-data")) {
              left -= $(this).find(".horizontal-scroll").scrollLeft();
          }
      });
        
      let pheader = $(document.body).find(".page-header").outerHeight();
      let scroll = $(window).scrollTop() + pheader;
      let wheight = $(window).outerHeight();
      let height = $(ctrl).outerHeight();
        
      if ((wheight - (top + height - scroll)) < 280) {
        a.css({
          "width": $(ctrl).outerWidth() + "px",
          "top": "auto",
          "bottom": (wheight - top + 3) + "px",
          "left": left + "px"
        });
      }
      else {
        a.css({
          "width": $(ctrl).outerWidth() + "px",
          "top": (top + height) + "px",
          "bottom": "auto",
          "left": left + "px"
        });
      }

      a.addClass("show");
    }, 0);
  }
  
  private filterData(fd) {
    let ls = [];
    let si = this.list.find((val) => {
      return val["selected"] == true;
    });

    let acache = AUTOCOMPLETE_CACHE[this.key];
    return new Promise((resolve) => {
      if (acache != undefined) {
        if (acache["cache"] == undefined) {
          acache["cache"] = [];
        }
  
        let ckey = "";
        if (fd.criteria != undefined) {
          for(let prop in fd.criteria) {
            if (ckey != "") {
              ckey += ":::";
            }
            ckey += fd.criteria[prop];
          }
        }

        let cache = acache["cache"].find((val) => {
          return val["key"] == ckey;
        });
        if (cache == undefined) {
          cache = {
            key: ckey,
            data: []
          };
          acache["cache"].push(cache);
  
          this.api.callApiController(acache["url"], {
            type: "POST",
            data: acache["data"]
          }).then((res) => {
            if (res != undefined) {
              cache["data"] = res;
              resolve(cache);
            }
            else {
              cache["data"] = null;
            }
          });
        }
        else {
          if (cache["data"] == undefined) {
            this.api.callApiController(acache["url"], {
              type: "POST"
            }).then((res) => {
              if (res != undefined) {
                cache["data"] = res;
                resolve(cache);
              }
              else {
                cache["data"] = null;
              }
            });
          }
          else {
            resolve(cache);
          }
        }
      }
    }).then((res) => {
      if (res == undefined) {
        return;
      }

      if ((fd.display != undefined && fd.disaply != "")
          || fd.value != undefined) {
        let fs = res["data"].filter((val) => {
          if (val == undefined) {
            return false;
          }
          
          if (fd.display != undefined) {
            let regt = this.escapeRegExp(fd.display);
            return new RegExp(regt, 'ig').test(val[acache["display"]] as string);
          }
          else {
            return val[acache["value"]] == fd.value;
          }
        }).map((val) => {
          return {
            display: val[acache["display"]],
            value: val[acache["value"]],
            data: val
          };
        });
        
        for(let i = 0; i < this.maxlist; i++) {
          if (fs.length <= i) {
            break;
          }
          ls.push(fs[i]);
        }
        if (si != undefined) {
          let lsi = ls.find((val) => {
            return val["value"] == si["value"];
          });
          if (lsi != undefined) {
            lsi["selected"] = true;
          }
        }
      }
      else {
        for(let i = 0; i < this.maxlist; i++) {
          if (res["data"].length <= i) {
            break;
          }

          let r = res["data"][i];
          ls.push({
            display: r[acache["display"]],
            value: r[acache["value"]],
            data: r
          });
        }
      }
      this.list = ls;
    });
  }
  private escapeRegExp(str) {
    return str.replace(/[-\/\\^$*+?.()|[\]{}]/g, "\\$&"); // $& means the whole matched string
  }
  private updateScroll(fIdx) {
    let menuContainer = this.divAutocomplete.nativeElement.querySelector('.autocomplete');
    if (!menuContainer) { return; }

    let choices = menuContainer.querySelectorAll('.item');
    if (choices.length < 1
        && fIdx < choices.length) { return; }

    let highlighted: any = choices[fIdx];
    if (!highlighted) { return; }

    let top: number = highlighted.offsetTop + highlighted.clientHeight - menuContainer.scrollTop;
    let height: number = menuContainer.offsetHeight;

    if (top > height) {
        menuContainer.scrollTop += top - height;
    } else if (top < highlighted.clientHeight) {
        menuContainer.scrollTop -= highlighted.clientHeight - top;
    }
  }
}
