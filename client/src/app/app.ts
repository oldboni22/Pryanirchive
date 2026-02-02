import { Component, signal } from '@angular/core';
import { Panel } from "./shared/panel/panel";
import { Button } from "./shared/button/button";
import {SizeDirective} from './shared/size-directive';
import {Icon} from './shared/icon/icon';
import {CurrentUserBanner} from './features/current-user-banner/current-user-banner';
import {Folder} from './features/folder/folder';
import {File} from './features/file/file';
@Component({
  selector: 'app-root',
  imports: [Panel, Button, SizeDirective, Icon, CurrentUserBanner, Folder, File],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('micro-forum');
  protected readonly console = console;
}

