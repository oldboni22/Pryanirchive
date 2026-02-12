import {Component, input, signal} from '@angular/core';
import {ControlValueAccessor} from '@angular/forms';

@Component({
  selector: 'app-input-field',
  templateUrl: './input-field.html',
  imports: [
  ]
})
export class InputField implements ControlValueAccessor {
  autocomplete = input<string>('');
  placeholder = input<string>('');
  type = input<string>('text');

  value = signal<string>('');
  isEnabled = signal<boolean>(false);

  showPassword = signal<boolean>(false);



  writeValue(obj: any): void {
    throw new Error("Method not implemented.");
  }
  registerOnChange(fn: any): void {
    throw new Error("Method not implemented.");
  }
  registerOnTouched(fn: any): void {
    throw new Error("Method not implemented.");
  }
  setDisabledState?(isDisabled: boolean): void {
    throw new Error("Method not implemented.");
  }
}
