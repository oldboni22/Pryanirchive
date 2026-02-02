import {Component, input} from '@angular/core';
import {Panel} from '../../../shared/panel/panel';
import {Icon} from '../../../shared/icon/icon';
import {InfoContentPanel} from './info-content-panel/info-content-panel';
import {FileRead} from '../../../core/models/file-read';
import {SizeDirective} from '../../../shared/size-directive';
import {Button} from '../../../shared/button/button';

@Component({
  selector: 'app-file-preview',
  imports: [
    Panel,
    Icon,
    InfoContentPanel,
    SizeDirective,
    Button
  ],
  templateUrl: './file-preview.html',
  styleUrl: './file-preview.css',
})
export class FilePreview {
  selectedFile = input<FileRead | null>(null);
}
