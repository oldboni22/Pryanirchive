import { Component } from '@angular/core';
import {Box} from '../../components/box/box';
import {scan} from 'rxjs';
import {Header} from '../../components/header/header';
import {InputField} from '../../components/input-field/input-field';

@Component({
  selector: 'app-login-screen',
  imports: [
    Box,
    Header,
    InputField
  ],
  templateUrl: './login-screen.html',
})
export class LoginScreen {

  protected readonly scan = scan;
}
