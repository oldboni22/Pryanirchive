import {Component, computed, input} from '@angular/core';
import {InlineButton} from '../../../components/inline-button/inline-button';

@Component({
  selector: 'app-user-button',
  imports: [
    InlineButton
  ],
  host: {
    '[class]': 'combinedClasses()',
  },
  styles: ':host{display: block}',
  templateUrl: './user-button.html'
})
export class UserButton {
  class = input<string>('', { alias: 'class' });
  protected readonly console = console;

  combinedClasses = computed(() => {
    return [
      this.class(),
      `select-none cursor-pointer
      transition
      shadow-user
      active:shadow-user-active
      hover:shadow-user-hover
      bg-user-gradient
      text-inherit text-center
      rounded-r-2xl`
    ].filter(Boolean).join(' ');
  });
}
