import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EnrollmentFormComponent } from './enrollment-form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CourseService } from '@features/courses/course.service';
import { EnrollmentService } from '../enrollment.service';
import { of } from 'rxjs';
import { FormControl } from '@angular/forms';

describe('EnrollmentFormComponent', () => {
  let component: EnrollmentFormComponent;
  let fixture: ComponentFixture<EnrollmentFormComponent>;
  let courseServiceSpy: jasmine.SpyObj<CourseService>;
  let enrollmentServiceSpy: jasmine.SpyObj<EnrollmentService>;

  beforeEach(async () => {
    courseServiceSpy = jasmine.createSpyObj('CourseService', ['getCourses']);
    enrollmentServiceSpy = jasmine.createSpyObj('EnrollmentService', ['createEnrollment']);

    await TestBed.configureTestingModule({
      imports: [
        EnrollmentFormComponent,
        HttpClientTestingModule,
        MatSnackBarModule
      ],
      providers: [
        { provide: CourseService, useValue: courseServiceSpy },
        { provide: EnrollmentService, useValue: enrollmentServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EnrollmentFormComponent);
    component = fixture.componentInstance;

    courseServiceSpy.getCourses.and.returnValue(of([]));
    fixture.detectChanges();
  });

  it('debería crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debería inicializar el formulario con valores por defecto', () => {
    expect(component.form).toBeDefined();
    expect(component.form.get('studentName')?.value).toBe('');
    expect(component.form.get('selectedCourses')?.value).toEqual([]);
  });

  it('debería invalidar si nombre es vacío', () => {
    component.form.get('studentName')?.setValue('');
    expect(component.form.get('studentName')?.valid).toBeFalse();
  });

  it('debería validar nombre mínimo 3 letras', () => {
    component.form.get('studentName')?.setValue('ab');
    expect(component.form.get('studentName')?.valid).toBeFalse();

    component.form.get('studentName')?.setValue('abc');
    expect(component.form.get('studentName')?.valid).toBeTrue();
  });

  it('debería ser inválido si no se selecciona ningún curso', () => {
    component.form.get('studentName')?.setValue('Daniel');
    component.form.get('selectedCourses')?.setValue([]);
    expect(component.form.valid).toBeFalse();
  });

  it('debería ser válido con nombre correcto y 1 curso', () => {
    component.form.get('studentName')?.setValue('Daniel');
    (component.form.get('selectedCourses') as any).push(new FormControl('1'));
    expect(component.form.valid).toBeTrue();
  });
});