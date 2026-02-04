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

  onClick = output<MouseEvent>();

  @HostListener('click', ['$event'])
  handleClick(event: MouseEvent) {
    this.onClick.emit(event);
  }

  combinedClasses = computed(() => {
    return [
      this.class(),
      `hover:shadow-volume
       active:bg-accent-color active:bg-button-shiny
       select-none cursor-pointer text-(--text-color)`
    ].filter(Boolean).join(' ');
  });
}
