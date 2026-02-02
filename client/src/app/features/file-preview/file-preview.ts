import { Component } from '@angular/core';
import {Panel} from '../../shared/panel/panel';
import {Icon} from '../../shared/icon/icon';
import {InfoContentPanel} from './info-content-panel/info-content-panel';

@Component({
  selector: 'app-file-preview',
  imports: [
    Panel,
    Icon,
    InfoContentPanel
  ],
  templateUrl: './file-preview.html',
  styleUrl: './file-preview.css',
})
export class FilePreview {

}
