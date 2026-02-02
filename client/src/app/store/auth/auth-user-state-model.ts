import { UserReadModel } from "../../core/models/user-read-model";

export interface AuthUserStateModel{
    user: UserReadModel | null;
    isLoading: boolean;
}
