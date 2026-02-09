import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Box} from './components/box/box';
import {Header} from './components/header/header';
import {InlineButton} from './components/inline-button/inline-button';
import {Button} from './components/button/button';
import {FileExplorer} from './features/file-explorer/file-explorer';
import {UserButton} from './features/task-bar/user-button/user-button';
import {Tile} from './features/task-bar/tile/tile';
import {LoginScreen} from './features/login-screen/login-screen';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Box, Header, InlineButton, Button, FileExplorer, UserButton, Tile, LoginScreen],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('soy-files');
  protected readonly console = console;

  test_login = true;
}
