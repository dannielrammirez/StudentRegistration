import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Course } from '@shared/models/course';
import { CourseService } from '@features/courses/course.service';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})

export class CourseListComponent implements OnInit {
  courses$!: Observable<Course[]>;

  constructor(private courseService: CourseService) { }

  ngOnInit(): void {
    this.courses$ = this.courseService.getCourses();
  }
}