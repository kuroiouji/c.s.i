import { Component, Input, Output, ViewEncapsulation, forwardRef, ElementRef, EventEmitter, ViewChild, HostListener } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR  } from "@angular/forms";

import { tabToControl, textDateLocale, cloneObject } from "../../functions";
import { ResponseMessage } from "../../models";
import { CtmDialogComponent } from "../dialog/dialog.component";

let moment = require("moment");

export type Type = 'date' | 'time' | 'datetime' | 'month' | 'year';

export const CTM_DATEPICKER_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CtmDatePickerComponent),
  multi: true
};

@Component({
  selector: 'ctm-datepicker',
  templateUrl: './datepicker.component.html',
  providers: [CTM_DATEPICKER_CONTROL_VALUE_ACCESSOR],
  host: {   
  },
  encapsulation: ViewEncapsulation.None
})
export class CtmDatePickerComponent implements ControlValueAccessor {
  @ViewChild(CtmDialogComponent) dialogCtrl: CtmDialogComponent;

  private _inputValue: string = "";
  private _value: any = "";
  private _readonly: boolean = false;
  private _error: ResponseMessage = new ResponseMessage();
  private isFocused: boolean = false;
  
  private _format: string;
  private calendarDays: Array<Object> = [];
  private calendarMonths: Array<Object> = [];
  private calendarYears: Array<Object> = [];
  private selectedDate: Date;
  private showCalendar: boolean = true;
  private showCalendarMonth: boolean = false;
  private showCalendarYear: boolean = false;
  private _min: any = null;
  private _max: any = null;

  constructor(
    private element: ElementRef
  ) { 
  }

  @HostListener("window:resize")
  public onWindowResize():void {
    this.dialogCtrl.close();
  }
  @HostListener("window:scroll")
  public onWindowScroll():void {
    this.dialogCtrl.close();
  }

  writeValue(value: any): void {
    if (value != undefined) {
      if (!(value instanceof Date)) {
        if (this.type == "time") {
            let vc = moment(value, this.format, this.locale);
            if (vc.isValid()) {
                value = vc.toDate();
            }
        }
        else {
            let vc = moment(value, null, this.locale);
            if (vc.isValid()) {
                value = vc.toDate();
            }
        }
      }

      if (this.type == "time"
          || this.type == "datetime") {
          if (this.timeRange > 1) {
              let cm = value.getMinutes();
              let ncm = Math.floor(value.getMinutes() / this.timeRange) * this.timeRange;
              if (cm != ncm) {
                  value.setMinutes(ncm);
                  value.setSeconds(0);

                  this.value = value;
                  this._inputValue = textDateLocale(value, this.format, this.locale);

                  this._onChange(this.value);
                  this.change.emit({
                    source: this,
                    value: this.value
                  });
                  return;
              }
          }
      }
    
      this._value = value;
      this._inputValue = textDateLocale(value, this.format, this.locale);
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
  public placeholder: string = "";
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
    this._onChange(value);
  }
  
  @Input()
  public type: Type = "date";
  @Input()
  public get format() {
    return this._format
      || (this.type === "date"? "DD/MM/YYYY"
          : this.type === "time"? "HH:mm"
          : this.type === "datetime"? "DD/MM/YYYY HH:mm"
          : this.type === "month"? "MM/YYYY"
          : this.type === "year"? "YYYY"
          : "DD/MM/YYYY");
  }
  public set format(value: string) {
      if (this._format !== value) { this._format = value; }
  }
  @Input()
  public locale: string = "en";
  @Input()
  public timeRange: number = 1;
  @Input("default-value")
  public defaultValue: Date;
  
  @Input() 
  public enableDates: Array<Date> = [];
  @Input() 
  public disableDates: Array<Date> = [];
  @Input() 
  public disableDays: Array<number> = [];
  @Input("disable-weekdays") 
  public disableWeekDays: Array<number> = [];
  
  @Input()
  set min(value: any) {
    if (value) {
      this._min = value;
  
      if (this._min) {
        let min = this._min.value as Date;
        if (min) {
          min = moment(min).toDate();
                  
          if (this.value < min) {
            let v = new Date(min);
            if (this.type !== "time" && this.type !== "datetime") {
                v.setHours(0, 0, 0, 0);
            }
          
            this.value = v;
            this._inputValue = textDateLocale(v, this.format, this.locale);
          }
        }
      }
    }
    else {
      this._min = null;
    }
  }
  @Input("allow-change-min")
  public allowChangeMin: boolean = true;

  @Input()
  set max(value: any) {
    if (value) {
      this._max = value;
  
      let max = this._max.value as Date;
      if (max != undefined) {
        max = moment(max).toDate();
          
        if (this.value > max) {
          let v = new Date(max);
          if (this.type !== "time" && this.type !== "datetime") {
              v.setHours(0, 0, 0, 0);
          }
      
          this.value = v;
          this._inputValue = textDateLocale(v, this.format, this.locale);
        }
      }
    }
    else {
      this._max = null;
    }
  }
  @Input("allow-change-max")
  public allowChangeMax: boolean = true;

  @Output() 
  public focus: EventEmitter<any> = new EventEmitter<any>();
  @Output() 
  public blur: EventEmitter<any> = new EventEmitter<any>();
  @Output() 
  public change: EventEmitter<any> = new EventEmitter<any>();

  private get inputValue() {
    return this._inputValue;
  }
  private set inputValue(value: string) {
    if (value == "") {
      if (this.value != undefined) {
        if (this.defaultValue) {
            this.value = this.defaultValue;
        }
        else {
            this.value = null;
        }

        this._onChange(this.value);
        this.change.emit({
          source: this,
          value: this.value
        });
      }
      return;
    }

    let m = moment(value, this.format, this.locale, true);
    if (m.isValid() == false) {
      m = moment(value, 
          this.format
              .replace(/\//g,'')
              .replace(/:/g,'')
              .replace(/ /g,''), this.locale, true);
    }
          
    if (m.isValid() == false) {
      if (this.value != undefined) {
        if (this.defaultValue) {
            this.value = this.defaultValue;
        }
        else {
            this.value = null;
        }

        this._onChange(this.value);
        this.change.emit({
          source: this,
          value: this.value
        });
      }
    }
    else {
      if (this.locale == "th") {
        m.year(m.year() - 543);
      }
      if (this.type == "time"
          || this.type == "datetime") {
        if (this.timeRange > 0) {
            m.minutes(Math.floor(m.minutes() / this.timeRange) * this.timeRange)
            m.seconds(0);
        }
      }

      if (this.disableDates.length > 0) {
        for (let d of this.disableDates) {
          if (moment(m.toDate()).format('YYYYMMDD') == moment(d).format('YYYYMMDD')) { 
            this.value = null;
            this._onChange(this.value);
            this.change.emit({
              source: this,
              value: this.value
            });

            return;
          }
        }
      }
      if (this.disableDays.length > 0) {
        for (let d of this.disableDays) {
          if (m.toDate().getDate() === d) {
            this.value = null;
            this._onChange(this.value);
            this.change.emit({
              source: this,
              value: this.value
            });

            return;            
          }
        }
      }

      if (this.disableWeekDays.length > 0) {
        for (let d of this.disableWeekDays) {
          if (m.toDate().getDay() === d) {
            let wds = [0,1,2,3,4,5,6,7].filter((value) => {
              return this.disableWeekDays.filter((v) => {
                  return v == value;
              }).length == 0;
            });
            if (wds.length == 0) {
              this.value = null;
              this._onChange(this.value);
              this.change.emit({
                source: this,
                value: this.value
              });

              return;
            }
            else {
              let refIdx = -1;
              for(let idx = 0; idx < wds.length; idx++) {
                if (wds[idx] > d) {
                  refIdx = idx;
                  break;
                }
              }
              let nd = m.toDate().getDate() - m.toDate().getDay();
              if (refIdx == -1) {
                nd += wds[0] + 7;
              }
              else {
                nd += wds[refIdx];
              }
          
              m = moment(m.toDate().setDate(nd));
            }              
          }
        }
      }
      if (this._min) {
        let min = this._min.value as Date;
        if (min) {
          min = moment(min).toDate();

          if (this.allowChangeMin == false) {
            if (min && min > m.toDate()) {
              this.value = null;
              this._onChange(this.value);
              this.change.emit({
                source: this,
                value: this.value
              });

              return;
            }
          }
          else {
            if (min && min > m) {
              this._min.setValue(m);
            }
          }
        }
      }
      if (this._max) {
        let max = this._max.value as Date;
        if (max) {
          max = moment(max).toDate();

          if (this.allowChangeMax == false) {
              if (max && max < m.toDate()) {
                this.value = null;
                this._onChange(this.value);
                this.change.emit({
                  source: this,
                  value: this.value
                });

                return;
              }
          }
          else {
            if (max && max < m) {
              this._max.setValue(m);
            }
          }
        }
      }

      this.value = m.toDate();
      this._onChange(this.value);
      this.change.emit({
        source: this,
        value: this.value
      });
    }
  }

  public setFocus() {
    this.isFocused = true;
    this.element.nativeElement.querySelector('input').focus();
  }

  private onKeypress(event) {
    if (event.keyCode == 13) {
      if (this.value == undefined) {
        this._inputValue = null;
      }
      else {
        this._inputValue = textDateLocale(this.value, this.format, this.locale);
      }
      
      if (this.tabWithEnter) {
          tabToControl(event);

          return false;
      }      
    }
    else {
      let txt = String.fromCharCode(event.keyCode);
      if (txt != "/" 
          && isNaN(parseInt(txt)) == true) {
          if (txt == ":"
              && (this.type == "time"
                  || this.type == "datetime")) {
              //Skip.
          }
          else {
              return false;
          }
      }
    }
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
    if (this.value == undefined) {
      this._inputValue = null;
    }
    else {
      this._inputValue = textDateLocale(this.value, this.format, this.locale);
    }

    this.isFocused = false;
    
    this._onTouched();
    this.blur.emit();
  }

  private get isRequired() {
    return this.disabled == false
            && this.required == true;
  }

  private onTrickerDatePicker() {
    let m = moment(this._value);
    if (m.isValid()) {
      this.selectedDate = m.toDate();
    }
    else {
      let cc = 1;
      let d = new Date();
      while (this.disabledDate(d)) {
        d.setDate(d.getDate() + 1);

        cc++;
        if (cc > 30) {
          break;
        }
      }
      this.selectedDate = d;
    }
    

    this.generateCalendarYear();
    this.generateCalendarMonth();
    this.generateCalendar();

    this.showCalendar = true;
    this.showCalendarMonth = false;
    this.showCalendarYear = false;
    this.dialogCtrl.open();
  }
  private get dayNameText() {
    if (this.selectedDate != undefined) {
      return moment(this.selectedDate).format("dddd");
    }

    return "";
  }
  private get dayText() {
    if (this.selectedDate != undefined) {
      return moment(this.selectedDate).format("D");
    }

    return "";
  }
  private get monthText() {
    if (this.selectedDate != undefined) {
      return moment(this.selectedDate).format("MMMM");
    }

    return "";
  }
  private get yearText() {
    if (this.selectedDate != undefined) {
      return moment(this.selectedDate).format("YYYY");
    }

    return "";
  }

  private generateCalendar() {
    let today = moment(new Date()).format('YYYYMMDD');
    let sday = moment(this.selectedDate).format('YYYYMMDD');

    let calendar = [];
    let date = new Date(this.selectedDate.getFullYear(), this.selectedDate.getMonth(), 1);
    let year = date.getFullYear();
    let month = date.getMonth();
    
    let fday = date.getDay();
    date.setDate(date.getDate() + (fday > 0? (-1 * (fday)) : 0));
    
    for(let w = 0; w < 6; w++) {
        let week: Array<any> = [];
        for (let i = 0; i < 7; i++) {
            let type = "c";
            if (date.getMonth() != month) {
                type = (w == 0)? "p": "n";
            }
            
            week.push({
                type: type,
                date: date.getDate(),
                current: (today == moment(date).format('YYYYMMDD')),
                selected: (sday == moment(date).format('YYYYMMDD')),
                disabled: this.disabledDate(date)
            });

            date.setDate(date.getDate() + 1);
        }

        calendar.push(week);
    }

    this.calendarDays = calendar;
  }
  private disabledDate(date: Date): boolean {
    for (let d of this.enableDates) {
      if (moment(date).format('YYYYMMDD') == moment(d).format('YYYYMMDD')) { 
        return false; 
      }
    }
    for (let d of this.disableDates) {
      if (moment(date).format('YYYYMMDD') == moment(d).format('YYYYMMDD')) { 
        return true; 
      }
    }
    for (let d of this.disableDays) {
      if (date.getDate() === d) {
        return true;            
      }
    }

    for (let d of this.disableWeekDays) {
      if (date.getDay() === d) { 
        return true; 
      }
    }

    let isDateWithinRange = (date: Date, minDate: Date, maxDate: Date) => {
      let dateAtMidnight = moment(date).format('YYYYMMDD');
      let minDateAtMidnight = minDate != undefined? moment(minDate).format('YYYYMMDD') : null;
      let maxDateAtMidnight = maxDate != undefined? moment(maxDate).format('YYYYMMDD') : null;
  
      return (!minDateAtMidnight || minDateAtMidnight <= dateAtMidnight) &&
      (!maxDateAtMidnight || maxDateAtMidnight >= dateAtMidnight);
    };

    if (this.allowChangeMin == false
        && this.allowChangeMax == false) {
      let min = this._min.value as Date;
      if (min) {
          min = moment(min).toDate();
      }
      let max = this._max.val as Date;
      if (max) {
          max = moment(max).toDate();
      }

      return !isDateWithinRange(date, min, max);
    }
    else if (this.allowChangeMin == false) {
        let min = this._min.value as Date;
        if (min) {
            min = moment(min).toDate();
        }
        
        return !isDateWithinRange(date, min, null);
    }
    else if (this.allowChangeMax == false) {
        let max = this._max.val as Date;
        if (max) {
            max = moment(max).toDate();
        }

        return !isDateWithinRange(date, null, max);
    }
  
    return false;
  }
  private onShowCalendar() {
    this.showCalendar = true;
    this.showCalendarMonth = false;
    this.showCalendarYear = false;
  }
  private onSelectDay(d) {
    for(let idx = 0; idx < this.calendarDays.length; idx++) {
      let iw = this.calendarDays[idx];
      for(let wIdx = 0; wIdx < iw["length"]; wIdx++) {
        let id = iw[wIdx];
        if (id["date"] != d["date"]) {
          id["selected"] = false;
        }
      }
    }
    d["selected"] = true;

    let nd = moment(this.selectedDate).toDate();
    if (d["type"] == "p") {
      nd.setMonth(nd.getMonth() - 1);
    }
    if (d["type"] == "n") {
      nd.setMonth(nd.getMonth() + 1);
    }
    nd.setDate(d.date);

    if (this.type == "time"
        || this.type == "datetime") {
      if (this.timeRange > 0) {
          nd.minutes(Math.floor(nd.minutes() / this.timeRange) * this.timeRange)
          nd.seconds(0);
      }
    }

    if (this._min) {
      let min = this._min.value as Date;
      if (min) {
        min = moment(min).toDate();

        if (this.allowChangeMin == false) {
          if (min && min > nd.toDate()) {
            this.value = null;
            this._onChange(this.value);
            this.change.emit({
              source: this,
              value: this.value
            });

            return;
          }
        }
        else {
          if (min && min > nd) {
            this._min.setValue(nd);
          }
        }
      }
    }
    if (this._max) {
      let max = this._max.value as Date;
      if (max) {
        max = moment(max).toDate();

        if (this.allowChangeMax == false) {
            if (max && max < nd.toDate()) {
              this.value = null;
              this._onChange(this.value);
              this.change.emit({
                source: this,
                value: this.value
              });

              return;
            }
        }
        else {
          if (max && max < nd) {
            this._max.setValue(nd);
          }
        }
      }
    }

    this.value = nd;
    this._inputValue = textDateLocale(nd, this.format, this.locale);

    //this.generateCalendar();
    this.dialogCtrl.close();
  }
  private dayCss(d) {
    let css = "";
    if (d.type == "p") {
      css = "prev-month";
    }
    else if (d.type == "n") {
      css = "next-month";
    }
    else if (d.current == true) {
      css = "current";
    }

    if (d.selected == true) {
      css += " selected";
    }
    if (d.disabled == true) {
      css += " disabled";
    }
    
    return css;
  }

  private generateCalendarMonth() {
    let cmm = moment(new Date()).format('MMMM');
    let mm = moment(this.selectedDate).format('MMMM');

    this.calendarMonths = Array.apply(0, Array(12))
                            .map(function(_,i){
                              return {
                                month: i,
                                name: moment().month(i).format('MMMM'),
                                current: (moment().month(i).format('MMMM') == cmm),
                                selected: (moment().month(i).format('MMMM') == mm)
                              };
                            });
  }
  private onShowMonth() {
    this.showCalendar = false;
    this.showCalendarMonth = true;
    this.showCalendarYear = false;
  }
  private onSelectMonth(m) {
    for(let idx = 0; idx < this.calendarMonths.length; idx++) {
      let im = this.calendarMonths[idx];
      if (im["month"] != m["month"]) {
        im["selected"] = false;
      }
    }
    m["selected"] = true;

    this.selectedDate.setMonth(m.month);
    this.generateCalendar();

    this.showCalendar = true;
    this.showCalendarMonth = false;
    this.showCalendarYear = false;
  }
  private monthCss(m) {
    let css = "";
    if (m.current == true) {
      css += "current";
    }
    if (m.selected == true) {
      css += " selected";
    }

    return css;
  }


  private generateCalendarYear() {
    let cyear = (new Date()).getFullYear();
    let year = this.selectedDate.getFullYear();
    
    let years = [];
    for(let idx = year - 9; idx <= (year + 10); idx++) {
      years.push({
        year: idx,
        current: idx == cyear,
        selected: idx == year
      });
    }

    this.calendarYears = years;
  }
  private onShowYear() {
    this.showCalendar = false;
    this.showCalendarMonth = false;
    this.showCalendarYear = true;
  }
  private onSelectYear(y) {
    for(let idx = 0; idx < this.calendarYears.length; idx++) {
      let iy = this.calendarYears[idx];
      if (iy["year"] != y["year"]) {
        iy["selected"] = false;
      }
    }
    y["selected"] = true;

    this.selectedDate.setFullYear(y.year);
    this.generateCalendarMonth();
    this.generateCalendar();

    this.showCalendar = false;
    this.showCalendarMonth = true;
    this.showCalendarYear = false;
  }
  private onPrevYear() {
    let year = this.selectedDate.getFullYear();
    
    let yearEnd = this.calendarYears[0]["year"] - 1;
    let yearStart = yearEnd - 19;
    
    let years = [];
    for(let idx = yearStart; idx <= yearEnd; idx++) {
      years.push({
        year: idx,
        current: idx == year
      });
    }
    this.calendarYears = years;
  }
  private onNextYear() {
    let year = this.selectedDate.getFullYear();
    
    let yearStart = this.calendarYears[this.calendarYears.length - 1]["year"] + 1;
    let yearEnd = yearStart + 19;
    
    let years = [];
    for(let idx = yearStart; idx <= yearEnd; idx++) {
      years.push({
        year: idx,
        current: idx == year
      });
    }
    this.calendarYears = years;
  }
  private yearCss(y) {
    let css = "";
    if (y.current == true) {
      css += "current";
    }
    if (y.selected == true) {
      css += " selected";
    }

    return css;
  }

  private onCloseCalendar() {
    this.dialogCtrl.close();
  }
}
