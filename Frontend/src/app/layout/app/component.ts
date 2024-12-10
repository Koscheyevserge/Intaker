import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    template: `
    this site is listening to https://localhost:7024 as the backend
    <router-outlet></router-outlet>`
})
export class AppComponent {
}
