import { Routes } from '@angular/router';
import { LoginComponent } from '@features/auth/login/login.component';
import { RegisterComponent } from '@features/auth/register/register.component';
import { CourseListComponent } from '@features/courses/course-list/course-list.component';
import { EnrollmentFormComponent } from '@features/enrollment/enrollment-form/enrollment-form.component';
import { StudentListComponent } from '@features/students/student-list/student-list.component';
// import { AuthGuard } from '@features/auth/auth.guard';
import { AuthGuard } from '@features/auth/auth-guard';

export const routes: Routes = [
    // Rutas públicas
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    // Rutas protegidas por el AuthGuard
    { path: 'enroll', component: EnrollmentFormComponent, canActivate: [AuthGuard] },
    { path: 'students', component: StudentListComponent, canActivate: [AuthGuard] },
    { path: 'courses', component: CourseListComponent, canActivate: [AuthGuard] },

    // Redirecciones
    { path: '', redirectTo: '/enroll', pathMatch: 'full' },
    { path: '**', redirectTo: '/enroll' }
];