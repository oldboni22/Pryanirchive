import { Action, Selector, State, StateContext } from "@ngxs/store";
import { AuthUserStateModel } from "./auth-user-state-model";
import { Inject, Injectable } from "@angular/core";
import { LoadUser } from "./auth-user-actions";
import { UserService } from "../../core/services/user-service";

@State<AuthUserStateModel>({
    name: 'authUser',
    defaults: {
        user: null,
        isLoading: false
    }
})
@Injectable()
export class AuthUserState {
    constructor(@Inject(UserService) private userService: UserService) {}

    @Selector()
    static getUser(state: AuthUserStateModel) {
        return state.user;
    }

    @Action(LoadUser)
    loadUser(ctx: StateContext<AuthUserStateModel>) {
        ctx.patchState({ isLoading: true });

        const user = this.userService.getUser();

        ctx.patchState({ user, isLoading: false });
    }
}
