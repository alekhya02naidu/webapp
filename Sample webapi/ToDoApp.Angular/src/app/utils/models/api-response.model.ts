export interface ApiResponse<T> {
    status: number;
    data: T;
    errorCode: string;
}