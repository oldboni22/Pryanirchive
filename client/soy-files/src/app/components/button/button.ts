import {Component, computed, HostListener, input, output} from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  template: '<ng-content></ng-content>',
  host: {
    '[class]': 'combinedClasses()',
  },
  styles: ':host { display: inline-flex; }',
})
export class Button {
  class = input<string>('', { alias: 'class' });

  onClick = output<MouseEvent>();

  @HostListener('click', ['$event'])
  handleClick(event: MouseEvent) {
    this.onClick.emit(event);
  }

  combinedClasses = computed(() => {
    return [
      'flex items-center justify-center text-center',
      'cursor-pointer select-none transition-all duration-75',
      'rounded-[3px] border-2 text-(--text-color)',

      'bg-bg-color border-border-color shadow-volume',
      'bg-button-shiny bg-cover',

      'hover:brightness-105 hover:border-[var(--button-focus-border)]',

      'active:shadow-button-pressed active:translate-y-[1px] active:brightness-95',

      'focus-visible:outline-1 focus-visible:outline-dotted focus-visible:outline-offset-[-4px] focus-visible:outline-black/50',

      this.class()
    ].filter(Boolean).join(' ');
  });
}
