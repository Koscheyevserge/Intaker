import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import {
    QueryService,
    CommandService
} from './services';
import { SettingsModule } from '../settings.module';

@NgModule({
    imports: [
        HttpClientModule,
        SettingsModule
    ],
    providers: [
        QueryService, CommandService
    ],
})
export class ServicesModule { }
