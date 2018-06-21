/* tslint:disable: max-line-length */
import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 's', pathMatch: 'full' },
  { path: 's', loadChildren: './screens/screens.module#ScreensModule' },
  { path: 'login', loadChildren: "./screens/security/+login/login.module#LoginModule" }
];
