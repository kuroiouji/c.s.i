import { Component, Input, ElementRef, HostListener } from '@angular/core';

import { tabToControl } from '../../functions';

@Component({
  selector: 'ctm-dialog',
  templateUrl: './dialog.component.html'
})
export class CtmDialogComponent {
  private showDialog: boolean = false;
  private showCss: boolean = false;

  constructor(
    private element: ElementRef
  ) { }

  @HostListener("window:keydown", ['$event'])
  public onWindowScroll(event):void {
    if (this.showDialog == true) {
      if($(this.element.nativeElement).find($(event.target)).length == 0) {
        event.preventDefault();
        event.stopPropagation();

        tabToControl(event, $(this.element.nativeElement));
      }
    }
  }

  @Input("dialog-css")
  public dialogCss: string;

  public open() {
    this.showDialog = true;
    this.showCss = true;
  }
  public close() {
    return new Promise((resolve) => {
      this.showCss = false;
      setTimeout(() => {
        this.showDialog = false;

        resolve();
      }, 1000);
    });
  }
}
