import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { CourseListComponent } from './course-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CourseService } from '@features/courses/course.service';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';
import { Course } from '@shared/models/course';

describe('CourseListComponent', () => {
  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;
  let courseService: jasmine.SpyObj<CourseService>;

  const mockCourses: Course[] = [
    {
      id: '1',
      name: 'Matemáticas',
      credits: 3,
      idProfessor: 'a1',
      isActive: true,
      createdAt: '2024-01-01T00:00:00Z',
      professorName: 'Juan Ramírez'
    },
    {
      id: '2',
      name: 'Física',
      credits: 3,
      idProfessor: 'a2',
      isActive: true,
      createdAt: '2024-01-01T00:00:00Z',
      professorName: 'Luisa Gómez'
    }
  ];

  beforeEach(waitForAsync(() => {
    const courseServiceSpy = jasmine.createSpyObj('CourseService', ['getCourses']);

    TestBed.configureTestingModule({
      imports: [
        CourseListComponent,
        HttpClientTestingModule
      ],
      providers: [
        { provide: CourseService, useValue: courseServiceSpy }
      ]
    }).compileComponents();

    courseService = TestBed.inject(CourseService) as jasmine.SpyObj<CourseService>;
  }));

  beforeEach(() => {
    courseService.getCourses.and.returnValue(of(mockCourses));

    fixture = TestBed.createComponent(CourseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debería crear el componente', () => {
    expect(component).toBeTruthy();
  });

  it('debería cargar y mostrar la lista de materias', () => {
    const listItems = fixture.debugElement.queryAll(By.css('li'));
    expect(listItems.length).toBe(2);
    expect(listItems[0].nativeElement.textContent).toContain('Matemáticas');
    expect(listItems[1].nativeElement.textContent).toContain('Física');
  });

  it('debería mostrar mensaje si la lista está vacía', () => {
    courseService.getCourses.and.returnValue(of([]));
    fixture = TestBed.createComponent(CourseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    const emptyMsg = fixture.debugElement.query(By.css('ng-template#empty'));
    expect(emptyMsg).toBeTruthy();
  });
});