export interface UserCreateModel {
    name: string;
    password: string;
    avatarUrl: File | null;
}
