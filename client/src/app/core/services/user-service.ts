import { Injectable } from "@angular/core";
import { UserReadModel } from "../models/user-read-model";

@Injectable({providedIn: 'root'})
export class UserService {
    private mockUser: UserReadModel = {
        id: '1',
        name: 'Pryanik',
        tag: 'TEST',
        avatarUrl: 'assets/pryanik.jpg'
    };

    getUser() : UserReadModel | null {
        return this.mockUser;
    }

    updateUser(user: UserReadModel): UserReadModel {
        this.mockUser = user;
        return user;
    }
}
