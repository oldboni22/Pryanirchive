import {Component, computed, HostListener, input, output} from '@angular/core';

@Component({
  selector: 'app-inline-button',
  imports: [],
  template: '<ng-content></ng-content>',
  host: {
    '[class]': 'combinedClasses()',
  },
  styles: ':host { display: block; }',
})
export class InlineButton {
  class = input<string>('', { alias: 'class' });
  activeShiny = input<boolean>(true);
  hoverShadow = input<boolean>(true);
  onClick = output<MouseEvent>();

  @HostListener('click', ['$event'])
  handleClick(event: MouseEvent) {
    this.onClick.emit(event);
  }

  combinedClasses = computed(() => {
    return [
      this.class(),
      this.activeShiny() ? 'active:bg-button-shiny' : '',
      this.hoverShadow() ? 'hover:shadow-volume' : '',
      `select-none cursor-pointer)`
    ].filter(Boolean).join(' ');
  });
}
