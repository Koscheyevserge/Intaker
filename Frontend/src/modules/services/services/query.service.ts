import { inject } from "@angular/core";
import { BACKEND_URL } from "../../settings.module";
import { retry } from "rxjs";
import { HttpClient } from "@angular/common/http";

export class QueryService {
    http = inject(HttpClient);
    url = inject(BACKEND_URL);

    private readonly retryCount = 5;

    getTask = (id: number) => this.http.get(`${this.url}/tasks/get?id=${id}`).pipe(retry(this.retryCount));
    getTasks = () => this.http.get(`${this.url}/tasks/getAll`).pipe(retry(this.retryCount));
}