import {Component, input} from '@angular/core';
import {InlineButton} from '../../../components/inline-button/inline-button';

@Component({
  selector: 'app-toolbar-button',
  imports: [
    InlineButton
  ],
  templateUrl: './toolbar-button.html',
})
export class ToolbarButton {
  text = input.required<string>();
}
