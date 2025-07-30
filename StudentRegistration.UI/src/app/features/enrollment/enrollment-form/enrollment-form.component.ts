import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';

// Importaciones de Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

// Servicios y Modelos
import { Course } from '@shared/models/course';
import { CourseService } from '@features/courses/course.service';
import { EnrollmentService } from '@features/enrollment/enrollment.service';

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatCheckboxModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './enrollment-form.component.html',
  styleUrls: ['./enrollment-form.component.scss']
})
export class EnrollmentFormComponent implements OnInit {
  form: FormGroup;
  allCourses: Course[] = [];
  validationError: string | null = null;
  isLoading = false;

  get selectedCoursesGroup(): FormGroup {
    return this.form.get('selectedCourses') as FormGroup;
  }

  get selectedCoursesCount(): number {
    if (!this.selectedCoursesGroup.value) {
      return 0;
    }
    return Object.values(this.selectedCoursesGroup.value).filter(value => value === true).length;
  }

  constructor(
    private fb: FormBuilder,
    private courseService: CourseService,
    private enrollmentService: EnrollmentService
  ) {
    this.form = this.fb.group({
      selectedCourses: this.fb.group({})
    });
  }

  ngOnInit(): void {
    this.courseService.getCourses().subscribe(courses => {
      this.allCourses = courses;
      courses.forEach(course => {
        this.selectedCoursesGroup.addControl(course.id, this.fb.control(false));
      });
    });

    this.selectedCoursesGroup.valueChanges.subscribe(() => {
      this.validateSelection();
    });
  }

  validateSelection(): void {
    const selectedCourseIds = Object.keys(this.selectedCoursesGroup.value)
      .filter(key => this.selectedCoursesGroup.value[key]);

    if (selectedCourseIds.length > 3) {
      this.validationError = 'Error: No puedes seleccionar más de 3 materias.';
      return;
    }
    
    const selectedFullCourses = this.allCourses.filter(course => selectedCourseIds.includes(course.id));
    const professorIds = selectedFullCourses.map(course => course.professor.id);
    const uniqueProfessorIds = new Set(professorIds);

    if (professorIds.length !== uniqueProfessorIds.size) {
      this.validationError = 'Error: No puedes tener dos materias con el mismo profesor.';
      return;
    }

    this.validationError = null;
  }

  onSubmit(): void {
    const selectedCourseIds = Object.keys(this.selectedCoursesGroup.value)
      .filter(key => this.selectedCoursesGroup.value[key]);

    if (this.validationError || selectedCourseIds.length === 0) {
      return;
    }

    this.isLoading = true;
    
    this.enrollmentService.createEnrollment(selectedCourseIds).subscribe({
      next: () => {
        alert('¡Inscripción exitosa!');
        this.isLoading = false;
        this.selectedCoursesGroup.reset();
      },
      error: (err) => {
        alert(`Error en la inscripción: ${err.error?.message || 'Error desconocido'}`);
        this.isLoading = false;
      }
    });
  }
}