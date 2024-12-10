import { InjectionToken, NgModule } from '@angular/core';

export const BACKEND_URL = new InjectionToken<string>('BACKEND_URL');

@NgModule({
    providers: [
        {
            provide: BACKEND_URL, useValue: "https://localhost:7024"
        }
    ],
})
export class SettingsModule { }
