import { Component, computed, input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-box',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    '[class]': 'combinedClasses()',
  },
  styles: ':host { display: block; }',
  template:'<ng-content></ng-content>',
})
export class Box {
  class = input<string>('', { alias: 'class' });
  fill = input<boolean>(false);
  bordered = input<boolean>(false);
  transparent = input<boolean>(false);

  combinedClasses = computed(() => {
    return [
      this.fill() ? 'flex-1 w-full' : 'w-fit',
      this.bordered()
        ? `border-border-color border-[6px] rounded-lg bg-none
         shadow-volume transition-all duration-300 overflow-hidden`
        : '',
      this.transparent() ? 'bg-transparent' : 'bg-bg-color',
      this.class()
    ].filter(Boolean).join(' ');
  });
}
