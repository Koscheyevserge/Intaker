import { RouterModule, Routes } from '@angular/router';
import * as Workspace from './components';
import { NgModule } from '@angular/core';
import { ServicesModule } from '../services';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { MaterialModule } from '../material.module';

export const AppRoutes: Routes = [{
    path: '',
    redirectTo: 'tasks',
    pathMatch: 'full'
}, {
    path: 'tasks',
    component: Workspace.TasksListComponent
}
];

@NgModule({
    imports: [
        ServicesModule,
        AsyncPipe,
        JsonPipe,
        MaterialModule
    ],
    declarations: [
        Workspace.TasksListComponent
    ],
    exports: [RouterModule]
})
export class WorkspacesModule { }