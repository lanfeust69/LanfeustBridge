import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ServerModule } from '@angular/platform-server';
import { sharedConfig } from './app.module.shared';

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        ServerModule,
        FormsModule,
        ...sharedConfig.imports
    ],
    providers: sharedConfig.providers
})
export class AppModule {
}
