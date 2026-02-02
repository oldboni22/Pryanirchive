import { InjectionToken } from '@angular/core';

export interface AppConfig {
  noMedia: string;
  defaultFileIcon: string;
  folderIcon: string;
  defaultAvatar: string;
}

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');
