import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {NgxsModule, provideStore} from '@ngxs/store';
import {APP_CONFIG} from './app.config.constants';
import {APP_DI_CONFIG} from './app.config.constants.values';
import {AuthUserState} from './store/auth/auth-user-state';

export const appConfig: ApplicationConfig = {
  providers: [
    {provide: APP_CONFIG, useValue: APP_DI_CONFIG},
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideStore([
      AuthUserState
    ]),
  ]
};
