import { NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy, CommonModule } from '@angular/common';
import { AppRoutes, WorkspacesModule } from '../modules';
import * as Layout from './layout';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter, RouterModule } from '@angular/router';

@NgModule({
    exports: [
        Layout.AppComponent
    ],
    declarations: [
        Layout.AppComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CommonModule,
        WorkspacesModule,
        RouterModule,
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        provideAnimations(),
        provideRouter(AppRoutes)
    ],
    bootstrap: [Layout.AppComponent],
    entryComponents: [],
})
export class AppModule { }
