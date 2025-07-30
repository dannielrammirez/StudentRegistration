import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentListComponent } from './student-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StudentService } from '../student.service';
import { of } from 'rxjs';

describe('StudentListComponent', () => {
  let component: StudentListComponent;
  let fixture: ComponentFixture<StudentListComponent>;
  let studentService: jasmine.SpyObj<StudentService>;

  beforeEach(async () => {
    const serviceSpy = jasmine.createSpyObj('StudentService', ['getStudents']);

    await TestBed.configureTestingModule({
      imports: [StudentListComponent, HttpClientTestingModule],
      providers: [{ provide: StudentService, useValue: serviceSpy }]
    }).compileComponents();

    studentService = TestBed.inject(StudentService) as jasmine.SpyObj<StudentService>;
    studentService.getStudents.and.returnValue(of([]));

    fixture = TestBed.createComponent(StudentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debería crearse correctamente', () => {
    expect(component).toBeTruthy();
  });

  it('debería llamar al servicio al iniciar', () => {
    expect(studentService.getStudents).toHaveBeenCalled();
  });
});
