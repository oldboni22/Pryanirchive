import {Component, inject, input} from '@angular/core';
import {Button} from '../../shared/button/button';
import {Icon} from '../../shared/icon/icon';
import {SizeDirective} from '../../shared/size-directive';
import {APP_CONFIG} from '../../app.config.constants';
import {FolderRead} from '../../core/models/folder-read';


@Component({
  selector: 'app-folder',
  imports: [
    Button,
    Icon,
    SizeDirective
  ],
  templateUrl: './folder.html',
  styleUrl: './folder.css',
})
export class Folder {
  private config = inject(APP_CONFIG);

  readonly iconPath = this.config.folderIcon;

  folder = input.required<FolderRead>();
}
