export class Tasks {
    id?: number = 0;
    userId: number = Number(localStorage.getItem('USER_ID'));
    title: string = "";
    description?: string = "";
    isCompleted: boolean = false;
    createdAt: Date = new Date();
    updatedAt: Date = new Date();
}
