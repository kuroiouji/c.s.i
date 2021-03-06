import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { AuthGuard } from "../_common";

import { ScreensComponent } from "./screens.component";

export const routes: Routes = [
	{
		path: "",
        component: ScreensComponent,
        canActivate: [ AuthGuard ],
        children:[
            {
                path: 'u',
                loadChildren: './security/user/user.module#UserModule' 
            }
        ]
    }
]
export const routing: ModuleWithProviders = RouterModule.forChild(routes);