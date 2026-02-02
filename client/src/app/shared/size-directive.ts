import {Directive, ElementRef, input} from '@angular/core';

@Directive({
  standalone: true,
  selector: '[appSizeDirective]',
  host: {
    '[style.width]': 'width()',
    '[style.height]': 'height()'
  }
})
export class SizeDirective {
  width = input<string>();
  height = input<string>();
}
