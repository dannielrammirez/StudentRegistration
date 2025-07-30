import { TestBed } from '@angular/core/testing';
import { CourseService } from './course.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Course } from '@shared/models/course';

describe('CourseService', () => {
  let service: CourseService;
  let httpMock: HttpTestingController;

  const dummyCourses: Course[] = [
    {
      id: '1',
      name: 'Matemáticas',
      credits: 3,
      idProfessor: 'p1',
      isActive: true,
      createdAt: new Date().toISOString()
    },
    {
      id: '2',
      name: 'Física',
      credits: 3,
      idProfessor: 'p2',
      isActive: true,
      createdAt: new Date().toISOString()
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CourseService]
    });

    service = TestBed.inject(CourseService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería instanciar el servicio', () => {
    expect(service).toBeTruthy();
  });

  it('debería obtener lista de cursos (GET)', () => {
    service.getCourses().subscribe(courses => {
      expect(courses.length).toBe(2);
      expect(courses).toEqual(dummyCourses);
    });

    const req = httpMock.expectOne('https://localhost:5001/api/courses');
    expect(req.request.method).toBe('GET');
    req.flush(dummyCourses);
  });

  it('debería crear un curso (POST)', () => {
    const newCourse: Course = dummyCourses[0];

    service.createCourse(newCourse).subscribe(course => {
      expect(course).toEqual(newCourse);
    });

    const req = httpMock.expectOne('https://localhost:5001/api/courses');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newCourse);
    req.flush(newCourse);
  });

  it('debería actualizar un curso (PUT)', () => {
    const updatedCourse: Course = { ...dummyCourses[0], name: 'Matemáticas Avanzadas' };

    service.updateCourse(updatedCourse).subscribe(course => {
      expect(course.name).toBe('Matemáticas Avanzadas');
    });

    const req = httpMock.expectOne(`https://localhost:5001/api/courses/${updatedCourse.id}`);
    expect(req.request.method).toBe('PUT');
    req.flush(updatedCourse);
  });

  it('debería eliminar un curso (DELETE)', () => {
    const courseId = '1';

    service.deleteCourse(courseId).subscribe(response => {
      expect(response).toBeUndefined(); // void
    });

    const req = httpMock.expectOne(`https://localhost:5001/api/courses/${courseId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('debería obtener un curso por ID (GET)', () => {
    const courseId = '1';

    service.getCourseById(courseId).subscribe(course => {
      expect(course).toEqual(dummyCourses[0]);
    });

    const req = httpMock.expectOne(`https://localhost:44349/api/courses/${courseId}`);
    expect(req.request.method).toBe('GET');
    req.flush(dummyCourses[0]);
  });
});