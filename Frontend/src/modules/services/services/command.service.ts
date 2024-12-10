import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { retry } from "rxjs/operators";
import { BACKEND_URL } from "../../settings.module";

@Injectable()
export class CommandService {
    http = inject(HttpClient);
    url = inject(BACKEND_URL);

    private readonly retryCount = 5;

    public taskChangeStatus = (taskId: number, taskStatusId: number) =>
        this.http.post(`${this.url}/tasks/StatusChange`, { taskId, taskStatusId }).pipe(retry(this.retryCount));
}