import { Component } from '@angular/core';
import {Box} from '../../components/box/box';
import {FileExplorer} from '../file-explorer/file-explorer';
import {Header} from '../../components/header/header';
import {Tile} from '../task-bar/tile/tile';
import {UserButton} from '../task-bar/user-button/user-button';

@Component({
  selector: 'app-desktop',
  imports: [
    Box,
    FileExplorer,
    Header,
    Tile,
    UserButton
  ],
  templateUrl: './desktop.html',
})
export class Desktop {

}
