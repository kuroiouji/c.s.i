import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserSerchComponent } from './user-serch/user-serch.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserGroupSearchComponent } from './user-group-search/user-group-search.component';
import { UserGroupDetailComponent } from './user-group-detail/user-group-detail.component';

export const routes: Routes = [
    {
      path:'',
      redirectTo: 's',
      pathMatch: 'full'
    },
    {
      path: 's',
      component: UserSerchComponent
    },
    {
      path: 'd',
      component: UserDetailComponent
    },
    {
      path: 'gs',
      component: UserGroupSearchComponent
    },
    {
      path: 'gd',
      component: UserGroupDetailComponent
    }
  ];
  export const routing: ModuleWithProviders = RouterModule.forChild(routes);