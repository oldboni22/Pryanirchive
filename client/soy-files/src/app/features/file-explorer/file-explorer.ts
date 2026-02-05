import { Component } from '@angular/core';
import {Box} from '../../components/box/box';
import {Button} from '../../components/button/button';
import {InlineButton} from '../../components/inline-button/inline-button';
import {Header} from '../../components/header/header';
import {ToolbarButton} from './toolbar-button/toolbar-button';

@Component({
  selector: 'app-file-explorer',
  imports: [
    Box,
    Button,
    InlineButton,
    Header,
    ToolbarButton
  ],
  templateUrl: './file-explorer.html',
  styleUrl: './file-explorer.css',
})
export class FileExplorer {

  protected readonly console = console;
}
