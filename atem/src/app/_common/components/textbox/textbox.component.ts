import { Component, Input, Output, ViewEncapsulation, forwardRef, ElementRef, EventEmitter } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR  } from "@angular/forms";

import { tabToControl } from "../../functions";
import { ResponseMessage } from "../../models";

export const CTM_TEXTBOX_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CtmTextboxComponent),
  multi: true
};

@Component({
  selector: 'ctm-textbox',
  templateUrl: './textbox.component.html',
  providers: [CTM_TEXTBOX_CONTROL_VALUE_ACCESSOR],
  host: {   
  },
  encapsulation: ViewEncapsulation.None
})
export class CtmTextboxComponent implements ControlValueAccessor {
  
  private _value: any = "";
  private _readonly: boolean = false;
  private _error: ResponseMessage = new ResponseMessage();
  private isFocused: boolean = false;

  constructor(
    private element: ElementRef
  ) { 
  }

  writeValue(value: any): void {
    if (value !== this._value) {
      this._value = value;
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
  @Input("prefix")
  public prefix: string;
  @Input("suffix")
  public suffix: string;
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

  @Output() 
  public focus: EventEmitter<any> = new EventEmitter<any>();
  @Output() 
  public blur: EventEmitter<any> = new EventEmitter<any>();

  public setFocus() {
    this.isFocused = true;
    this.element.nativeElement.querySelector('input').focus();
  }

  private get usePrefix(): boolean {
    return this.prefix != undefined
            && this.prefix != "";
  }
  private get useSuffix(): boolean {
    return this.suffix != undefined
            && this.suffix != "";
  }

  private onKeypress(event) {
    if (event.keyCode == 13) {
      if (this.tabWithEnter) {
          tabToControl(event);

          return false;
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
    this.isFocused = false;

    this._onTouched();
    this.blur.emit();
  }

  private get isRequired() {
    return this.disabled == false
            && this.required == true;
  }
}
