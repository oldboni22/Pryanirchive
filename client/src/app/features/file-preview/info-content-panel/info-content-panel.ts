import {Component, input} from '@angular/core';

@Component({
  selector: 'app-info-content-panel',
  imports: [],
  templateUrl: './info-content-panel.html',
  styleUrl: './info-content-panel.css',
})
export class InfoContentPanel {
  label = input.required<string>();
  value = input.required<string>();
}
