import {Component, computed, inject, OnInit, output} from '@angular/core';
import { Button } from '../../shared/button/button';
import { Icon } from '../../shared/icon/icon';
import { SizeDirective } from '../../shared/size-directive';
import {Store} from '@ngxs/store';
import {AuthUserState} from '../../store/auth/auth-user-state';
import {LoadUser} from '../../store/auth/auth-user-actions';
import {APP_CONFIG} from '../../app.config.constants';

@Component({
  selector: 'app-current-user-banner',
  standalone: true,
  imports: [Button, Icon, SizeDirective],
  templateUrl: './current-user-banner.html',
  styleUrl: './current-user-banner.css',
})
export class CurrentUserBanner implements OnInit {
  private store = inject(Store);
  private config = inject(APP_CONFIG);

  user = this.store.selectSignal(AuthUserState.getUser);

  avatarUrl = computed(
    () => this.user()!.avatarUrl ?? this.config.defaultAvatar)

  profileOpened = output<void>();


  ngOnInit() {
    this.store.dispatch(new LoadUser());
  }

  // Событие для перехода в профиль


  onProfileClick() {
    console.log('Navigating to profile...');
    console.log(this.avatarUrl());
    this.profileOpened.emit();
  }
}
