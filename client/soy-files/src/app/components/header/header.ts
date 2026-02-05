import {Component, computed, input} from '@angular/core';

@Component({
  selector: 'app-header',
  host: {
    '[class]': "combinedClasses()",
  },
  imports: [],
  template: '<ng-content></ng-content>',
})
export class Header {
  class = input<string>('', { alias: 'class' });
  textPadding = input<boolean>(true);

  combinedClasses = computed(() => {
    return [
      this.class(),
      this.textPadding() ? 'px-2.5 md:px-4 xl:px-8' : '',
      `block flex items-center justify-between
       text-text-header font-bold text-xs select-none
       bg-header-bg shadow-header-highlight antialiased
       header-text-shadow`
    ].filter(Boolean).join(' ');
  });
}
