import {Component, computed, inject, input, output} from '@angular/core';
import {Button} from "../../shared/button/button";
import {Icon} from "../../shared/icon/icon";
import {SizeDirective} from "../../shared/size-directive";
import {APP_CONFIG, AppConfig} from '../../app.config.constants';
import {FileRead} from '../../core/models/file-read';

@Component({
  selector: 'app-file',
    imports: [
        Button,
        Icon,
        SizeDirective
    ],
  templateUrl: './file.html',
  styleUrl: './file.css',
})
export class File {
  private config = inject(APP_CONFIG);

  file = input.required<FileRead>();
  previewPath = computed<string>(
    () => this.file().previewUrl ?? this.config.defaultFileIcon);

  fileOutput = output<FileRead>();

  handleButtonClick() {
    const currentFile = this.file();
    console.log('Клик по файлу:', currentFile.name);

    this.fileOutput.emit(currentFile);
  }
}
