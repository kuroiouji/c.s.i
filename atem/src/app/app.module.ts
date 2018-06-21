/**
 * This module is the entry for your App.
 *
 * Make sure to use the 3 constant APP_ imports so you don't have to keep
 * track of your root app dependencies here. Only import directly in this file if
 * there is something that is specific to the environment.
 */

import { NgModule, ApplicationRef } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, PreloadAllModules } from '@angular/router';
import { removeNgStyles, createNewHosts, createInputTransfer } from '@angularclass/hmr';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { APP_DECLARATIONS } from './app.declarations';
import { APP_ENTRY_COMPONENTS } from './app.entry-components';
import { APP_IMPORTS } from './app.imports';
import { APP_PROVIDERS } from './app.providers';
import { AppState, InternalStateType } from './app.service';

import { routes } from './app.routing';

import { AppComponent } from './app.component';

type StoreType = {
  state: InternalStateType,
  restoreInputValues: () => void,
  disposeOldHosts: () => void
};

@NgModule({
  declarations: [
    AppComponent,
    APP_DECLARATIONS
  ],
  entryComponents: [APP_ENTRY_COMPONENTS],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    APP_IMPORTS,
    RouterModule.forRoot(routes, { useHash: false, preloadingStrategy: PreloadAllModules }),
  ],
  bootstrap: [AppComponent],
  exports: [AppComponent],
  providers: [APP_PROVIDERS]
})

export class AppModule {
  constructor(
    public appRef: ApplicationRef,
    public appState: AppState
  ) {
  }

  public hmrOnInit(store: StoreType) {
    if (!store || !store.state) {
      return;
    }
    //console.log('HMR store', JSON.stringify(store, null, 2));
    
    /**
     * Set state
     */
    this.appState._state = store.state;
    /**
     * Set input values
     */
    if ('restoreInputValues' in store) {
      let restoreInputValues = store.restoreInputValues;
      setTimeout(restoreInputValues);
    }

    this.appRef.tick();
    delete store.state;
    delete store.restoreInputValues;
  }

  public hmrOnDestroy(store: StoreType) {
    const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
    /**
     * Save state
     */
    const state = this.appState._state;
    store.state = state;
    /**
     * Recreate root elements
     */
    store.disposeOldHosts = createNewHosts(cmpLocation);
    /**
     * Save input values
     */
    store.restoreInputValues  = createInputTransfer();
    /**
     * Remove styles
     */
    removeNgStyles();
  }

  public hmrAfterDestroy(store: StoreType) {
    /**
     * Display new elements
     */
    store.disposeOldHosts();
    delete store.disposeOldHosts;
  }
}
