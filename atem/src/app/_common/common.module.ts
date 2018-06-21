import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { RouterModule }	from "@angular/router";
import { CtmApiService, CtmApiLoader, CtmMessageService, CtmTranslateService } from './services';

import { CtmDialogComponent, CtmMessageComponent, CtmTextboxComponent, CtmAutocompleteComponent,
          CtmDatePickerComponent } from './components';
import { CtmHighlightPipe, CtmDatePipe, CtmNumericPipe, CtmTranslatePipe } from "./pipes";

const COMMON_COMPONENTS = [
  CtmDialogComponent,
  CtmMessageComponent,
  CtmTextboxComponent,
  CtmAutocompleteComponent,
  CtmDatePickerComponent
];
const COMMON_PROVIDERS = [
];
const COMMON_PIPES = [
  CtmHighlightPipe,
  CtmDatePipe,
  CtmNumericPipe,
  CtmTranslatePipe
];
const COMMON_SERVICES = [
  CtmApiService,
  CtmApiLoader,
  CtmMessageService,
  CtmTranslateService
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    ...COMMON_PIPES,
    ...COMMON_COMPONENTS
  ],
  providers: [
    ...COMMON_PROVIDERS
  ],
  exports: [
    ...COMMON_PIPES,
    ...COMMON_COMPONENTS
  ],
  entryComponents: []
})
export class CtmModule { 
  static forRoot(): ModuleWithProviders {
    return <ModuleWithProviders>{
        ngModule: CtmModule,
        providers: [
          ...COMMON_SERVICES
        ]
    };
  }
}
