import {Component, computed, input} from '@angular/core';
import {InlineButton} from '../../../components/inline-button/inline-button';
import {Box} from '../../../components/box/box';

@Component({
  selector: 'app-tile',
  imports: [
    InlineButton,
    Box
  ],
  host: {
    '[class]': 'combinedClasses()',
  },
  styles: ':host{display: block}',
  templateUrl: './tile.html',
})
export class Tile {
  class = input<string>('', { alias: 'class' });

  label = input.required<string>();

  combinedClasses = computed(() => {
    return [
      this.class(),
      `select-none cursor-pointer
      transition
      bg-taskbar-tile
      border-1 border-border-color
      shadow-taskbar-tile hover:shadow-taskbar-tile-hover active:shadow-taskbar-tile-active
      text-inherit text-center
      rounded-md`
    ].filter(Boolean).join(' ');
  });
  protected readonly console = console;
}
