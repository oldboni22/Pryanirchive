import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Box} from './components/box/box';
import {Header} from './components/header/header';
import {InlineButton} from './components/inline-button/inline-button';
import {Button} from './components/button/button';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Box, Header, InlineButton, Button],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('soy-files');
  protected readonly console = console;
}
