import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

// Importaciones de Angular Material
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

// Servicios y Modelos
import { Student } from '@shared/models/student';
import { StudentService } from '../student.service';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit {
  dataSource = new MatTableDataSource<Student>();
  displayedColumns: string[] = ['firstName', 'lastName', 'email'];
  isLoading = true;

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.studentService.getClassmates().subscribe(students => {
      this.dataSource.data = students;
      this.isLoading = false;
    });
  }
}