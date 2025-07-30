/// <reference types="jasmine" />

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EnrollmentService } from './enrollment.service';

describe('EnrollmentService', () => {
  let service: EnrollmentService;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:44349/api/enrollments';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EnrollmentService]
    });

    service = TestBed.inject(EnrollmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería instanciar el servicio correctamente', () => {
    expect(service).toBeTruthy();
  });

  it('debería enviar una inscripción (POST)', () => {
    const enrollmentRequest = {
      studentName: 'Daniel Ramírez',
      selectedCourses: ['uuid-curso-1', 'uuid-curso-2']
    };

    service.createEnrollment(enrollmentRequest).subscribe(response => {
      expect(response).toBeTruthy(); // puedes validar contenido real si aplica
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(enrollmentRequest);
    req.flush({ success: true });
  });
});
