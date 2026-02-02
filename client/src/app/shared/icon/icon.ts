import { Component, computed, effect, inject, input, signal, untracked } from '@angular/core';
import { APP_CONFIG } from '../../app.config.constants';

@Component({
  selector: 'app-icon',
  standalone: true, // Хорошая практика для атомарных компонентов
  host: {
    /* Исправлено: класс добавляется, если embedded === true */
    '[class.embedded]': 'embedded()',
  },
  templateUrl: './icon.html',
  styleUrl: './icon.css',
})
export class Icon {
  private config = inject(APP_CONFIG);

  // Состояния
  hasError = signal<boolean>(false);

  // Входы
  path = input<string | null>(null);
  alt = input<string>("icon");
  embedded = input<boolean>(false);

  // Логика пути: если ошибка или нет пути — берем заглушку
  iconPath = computed(() => {
    if (this.hasError() || !this.path()) {
      return this.config.noMedia;
    }
    return this.path();
  });

  handleError() {
    if (!this.hasError()) {
      console.warn(`[IconComponent] Failed to load: ${this.path()}`);
      this.hasError.set(true);
    }
  }

  // Сброс ошибки при смене входных данных
  private resetError = effect(() => {
    this.path();
    untracked(() => this.hasError.set(false));
  });
}
