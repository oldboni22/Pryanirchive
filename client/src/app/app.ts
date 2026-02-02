import { Component, signal } from '@angular/core';
import { Panel } from "./shared/panel/panel";
import { Button } from "./shared/button/button";
import {SizeDirective} from './shared/size-directive';
import {Icon} from './shared/icon/icon';
import {CurrentUserBanner} from './features/current-user-banner/current-user-banner';
import {Folder} from './features/folder/folder';
import {File} from './features/file/file';
import {FilePreview} from './features/file-panel/file-preview/file-preview';
import {FileRead} from './core/models/file-read';
import {FilePanel} from './features/file-panel/file-panel';
@Component({
  selector: 'app-root',
  imports: [Panel, SizeDirective, Icon, CurrentUserBanner, Folder, File, FilePreview, FilePanel],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('micro-forum');
  protected readonly console = console;

  selectedFile = signal<FileRead | null>(null);

  onFileSelected(file: FileRead) {
    this.selectedFile.set(file);
  }

  onFileDeselected() {
    this.selectedFile.set(null);
  }
}

