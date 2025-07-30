# Proyecto de Registro Estudiantil - Prueba Técnica

## Resumen de la Aplicación

Aplicación Full-Stack de tipo Cliente-Servidor desarrollada con **.NET 9** y **Angular 17** que implementa un sistema de registro académico. La plataforma permite a los usuarios registrarse, iniciar sesión e inscribir materias bajo un conjunto de reglas de negocio específicas, como un límite de créditos y restricciones de profesorado.

La seguridad se gestiona mediante autenticación basada en Tokens JWT, protegiendo las rutas y la comunicación entre el cliente y el servidor.

---
## 🚀 Arquitectura y Puntos a Destacar

Esta aplicación no es solo un CRUD, sino una demostración de prácticas de desarrollo modernas.

### Backend (.NET 8)

* **Arquitectura Limpia (Clean Architecture)**: Se separaron las responsabilidades del sistema en capas (`Domain`, `Application`, `Infrastructure`, `Presentation`) para un bajo acoplamiento y alta cohesión.
* **Patrón CQRS con MediatR**: La lógica de negocio está aislada en `Commands` y `Queries` dentro de la capa de Aplicación, manteniendo los controladores extremadamente delgados y enfocados.
* **Unit of Work y Repositorio**: Se abstrajo el acceso a datos mediante estos patrones para centralizar la gestión de la base de datos y asegurar transacciones atómicas con Entity Framework Core.
* **Inyección de Dependencias**: Todo el sistema está construido sobre la inyección de dependencias, lo que facilita las pruebas y la mantenibilidad.
* **Manejo de Errores Global**: Un `Middleware` personalizado captura todas las excepciones no controladas y devuelve respuestas JSON estandarizadas y seguras.
* **Data Seeding con Bogus**: El API puebla automáticamente la base de datos con datos de prueba realistas y en español cada vez que se inicia en modo de desarrollo.
* **Documentación con Swagger**: Todos los endpoints están documentados para facilitar las pruebas y la integración.

### Frontend (Angular 17)

* **Arquitectura Standalone**: Se utilizó la arquitectura de componentes `standalone` más reciente para un diseño más simple y modular.
* **Formularios Reactivos**: Los formularios de login, registro e inscripción utilizan `ReactiveFormsModule` para un manejo robusto del estado y validaciones complejas.
* **Autenticación Segura**: El flujo de autenticación se maneja con:
    * **Route Guards**: Para proteger las rutas que requieren inicio de sesión.
    * **HTTP Interceptor**: Para adjuntar automáticamente el token JWT a todas las peticiones salientes al API.
* **Alias de Rutas (`tsconfig.json`)**: Se configuraron alias para importaciones limpias y mantenibles (`@features`, `@shared`).
* **Interfaz Atractiva**: Se utilizó **Angular Material** para construir una interfaz de usuario intuitiva y con una apariencia profesional.

---
## 🗃️ Configuración de la Base de Datos

**1. Cadena de Conexión**
El API está configurado por defecto para conectarse a un servidor SQL Server local.

* **Servidor**: `localhost\sqlexpress`
* **Nombre de la Base de Datos**: `DanielRamirez_PruebaInterRapidisimo`

Verifica que esta configuración en el archivo `appsettings.json` del proyecto API coincida con tu entorno.

**2. Creación de la Base de Datos**

Tienes dos opciones para crear y poblar la base de datos:

* **Opción A (Automática - Recomendada)**: Simplemente **ejecuta el proyecto API en modo de Desarrollo**. El sistema está configurado para **borrar, recrear y poblar la base de datos automáticamente** en cada inicio. Esto incluye la creación de profesores, materias y una cuenta de prueba.
    * **Usuario de Prueba**: `test@test.com`
    * **Contraseña**: `123456`

* **Opción B (Manual)**: Si prefieres crear la estructura manualmente, puedes ejecutar el script SQL provisto.
    * **Nombre del Script**: `[01_Create_Database_Schema].sql`
    * **Ubicación del Script**: El script se encuentra dentro de la carpeta del proyecto `StudentRegistration.Presentation.API`.

---
## ⚙️ Ejecución del Proyecto

1.  **Clonar el Repositorio**.
2.  **Backend**:
    * Abre la solución (`.sln`) en Visual Studio 2022.
    * Asegúrate de que la cadena de conexión en `appsettings.json` sea correcta.
    * Ejecuta el proyecto `StudentRegistration.Presentation.API`.
3.  **Frontend**:
    * Abre la carpeta `StudentRegistration.UI` en Visual Studio Code.
    * Abre una nueva terminal y ejecuta `npm install`.
    * Una vez finalizado, ejecuta `ng serve`.
4.  **Acceder a la Aplicación**:
    * Abre un navegador y ve a `http://localhost:4200`.
    * Inicia sesión con la cuenta de prueba mencionada en la sección de la base de datos.
