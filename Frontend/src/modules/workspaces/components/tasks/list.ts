import { Component } from '@angular/core';
import { CommandService, QueryService } from '../../../services';
import { Subject, merge, of, switchMap, tap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'tasks-list',
    template: `
    <div style="padding-top: 10px;"></div>
    <ng-container *ngIf="getAllTasks$ | async as tasks">
        <ng-container *ngFor="let task of tasks">
            <mat-card>
              <mat-card-content>
                <mat-card-header>
                  <mat-card-title>{{task.name}}</mat-card-title>
                  <mat-card-subtitle>{{task.description}}</mat-card-subtitle>
                </mat-card-header>
                <mat-card-content>
                    <p>Id: {{task.id}}</p>
                    <p>Status: {{task.statusName}}</p>
                </mat-card-content>
                <mat-card-actions>
                    <button mat-stroked-button color="primary" (click)="changeTaskStatus(task, 1)" [disabled]="task.statusId === 1">Not Started</button>
                    <button mat-stroked-button color="warn" (click)="changeTaskStatus(task, 2)" [disabled]="task.statusId === 2">In Progress</button>
                    <button mat-stroked-button color="accent" (click)="changeTaskStatus(task, 3)" [disabled]="task.statusId === 3">Completed</button>
                  </mat-card-actions>
              </mat-card-content>
            </mat-card>
        </ng-container>
    </ng-container>
    <div style="padding-bottom: 10px;"></div>
    `
})
export class TasksListComponent {
    constructor(private query: QueryService, private command: CommandService, private snack: MatSnackBar) {

    }

    commandExecuted$ = new Subject<void>();

    getAllTasks$ = merge(of(null), this.commandExecuted$).pipe(
        switchMap(() => this.query.getTasks())
    );

    changeTaskStatus(task: any, taskStatusId: number) {
        const s = this.changeTaskStatus$(task.id, taskStatusId).subscribe(() => {
            s.unsubscribe();
            task.taskStatusId = taskStatusId;
            switch (taskStatusId) {
                case 1:
                    task.statusName = 'Not Started';
                    break;
                case 2:
                    task.statusName = 'In Progress';
                    break;
                case 3:
                    task.statusName = 'Completed';
                    break;            
            }
            this.snack.open('Task status changed', 'Close', { duration: 2000 });
        });
    }

    private changeTaskStatus$ = (taskId: number, taskStatusId: number) => this.command.taskChangeStatus(taskId, taskStatusId).pipe(
        tap(() => this.commandExecuted$.next())
    );
}