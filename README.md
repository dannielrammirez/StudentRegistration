# PruebaTecnicaInterRapidisimo
Proyecto de Registro Estudiantil
Resumen de la Aplicación
Aplicación Full-Stack de tipo Cliente-Servidor desarrollada con .NET 8 y Angular 17 que simula un sistema de registro académico. La plataforma permite la gestión de estudiantes, materias y profesores, aplicando un conjunto de reglas de negocio específicas para el proceso de inscripción en un programa de créditos.

La seguridad se gestiona mediante autenticación basada en Tokens JWT, protegiendo las rutas y la comunicación entre el cliente y el servidor.

🚀 Arquitectura y Tecnologías
El proyecto fue diseñado siguiendo principios de software modernos para garantizar escalabilidad, mantenibilidad y un código limpio.

Backend (.NET 8)
Se implementó una Arquitectura Limpia (Clean Architecture) para separar las responsabilidades del sistema en capas bien definidas:

Domain: Contiene las entidades del negocio y las reglas más puras, sin dependencias externas.

Application: Orquesta los casos de uso y la lógica de la aplicación. Aquí se implementó el patrón CQRS con MediatR para desacoplar la lógica de los controladores. Se definen todos los contratos (interfaces) que las capas externas deben implementar.

Infrastructure: Contiene las implementaciones de los contratos. Se encarga del acceso a datos con Entity Framework Core (usando el patrón Unit of Work y Repositorio) y de servicios externos como el de seguridad para la generación de JWT.

Presentation (API): Una API RESTful "delgada" y segura que expone la funcionalidad al cliente. Los endpoints están documentados con Swagger/OpenAPI.

Además, el API cuenta con un manejador de errores global (Middleware) y un sistema de carga de datos inicial (Data Seeder) con datos aleatorios para el entorno de desarrollo.

Frontend (Angular 17)
Se desarrolló una Single-Page Application (SPA) utilizando la arquitectura Standalone más reciente de Angular.

Servicios: Se centralizó toda la comunicación con el API en servicios específicos para cada entidad, utilizando HttpClient.

Manejo de Estado Reactivo: Se utilizó RxJS y Observables para manejar los flujos de datos de forma asíncrona, creando una interfaz de usuario reactiva.

Autenticación: El flujo de login está protegido mediante Route Guards (para proteger las rutas) y un HTTP Interceptor (para adjuntar automáticamente el token JWT a todas las peticiones).

Diseño Limpio: Se usó SCSS para los estilos, siguiendo una estructura organizada y modular.

📋 Prerrequisitos
.NET 8 SDK o superior.

Node.js 20.x o superior.

Angular CLI 17 o superior.

SQL Server (Express, Developer, etc.).

Visual Studio 2022.

Visual Studio Code.

⚙️ Instalación y Ejecución
1. Clonar el Repositorio

Bash

git clone <URL_DE_TU_REPOSITORIO>
2. Configuración del Backend

Abrir la solución (.sln) en Visual Studio.

En el proyecto StudentRegistration.Presentation.API, abrir el archivo appsettings.json.

Modificar la cadena de conexión DefaultConnection para que apunte a tu instancia de SQL Server.

3. Ejecución del Backend

Seleccionar el proyecto StudentRegistration.Presentation.API como proyecto de inicio.

Presionar F5 o el botón de ejecutar. La API se iniciará y la base de datos se creará y poblará con datos aleatorios automáticamente. Se abrirá la interfaz de Swagger.

4. Configuración del Frontend

Abrir la carpeta StudentRegistration.UI en Visual Studio Code.

Abrir una nueva terminal y ejecutar npm install para instalar las dependencias.

5. Ejecución del Frontend

En la misma terminal, ejecutar ng serve.
