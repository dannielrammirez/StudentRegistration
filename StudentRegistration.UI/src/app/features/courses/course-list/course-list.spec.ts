import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CourseService } from '../course.service';
import { Course } from '../../../shared/models/course';
import { environment } from '../../../../environments/environment';

describe('CourseService', () => {
  let service: CourseService;
  let httpTestingController: HttpTestingController;
  const testUrl = `${environment.apiUrl}/courses`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CourseService]
    });
    service = TestBed.inject(CourseService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Después de cada prueba, verifica que no queden peticiones pendientes.
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve courses from the API via GET', () => {
    const mockCourses: Course[] = [
      { id: '1', name: 'Test Course 1', credits: 3, professor: { id: 'p1', fullName: 'Profesor Test' } },
      { id: '2', name: 'Test Course 2', credits: 3, professor: { id: 'p2', fullName: 'Profesora Test' } }
    ];

    // Llama al método del servicio. Aún no se ha hecho la petición real.
    service.getCourses().subscribe(courses => {
      // Esta parte se ejecuta DESPUÉS de que la petición simulada responde.
      expect(courses.length).toBe(2);
      expect(courses).toEqual(mockCourses);
    });

    // Ahora, esperamos que se haya hecho una petición a la URL correcta.
    const req = httpTestingController.expectOne(testUrl);

    // Verificamos que el método de la petición sea GET.
    expect(req.request.method).toEqual('GET');

    // Simulamos una respuesta exitosa del servidor, enviando nuestros datos falsos.
    req.flush(mockCourses);
  });
});