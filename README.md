# Proyecto de Registro Estudiantil - Prueba Técnica

## Resumen de la Aplicación

Aplicación Full-Stack de tipo Cliente-Servidor desarrollada con **.NET 9** y **Angular 17** que implementa un sistema de registro académico. La plataforma permite a los usuarios registrarse, iniciar sesión e inscribir materias bajo un conjunto de reglas de negocio específicas, como un límite de créditos y restricciones de profesorado.

La seguridad se gestiona mediante autenticación basada en Tokens JWT, protegiendo las rutas y la comunicación entre el cliente y el servidor.

---

## Requerimientos del Ejercicio

La aplicación fue desarrollada para dar respuesta punto por punto a los siguientes requerimientos:

1.  Realizar un CRUD que le permita a un usuario realizar un registro en línea.
2.  El estudiante se adhiere a un programa de créditos.
3.  Existen 10 materias.
4.  Cada materia equivale a 3 créditos.
5.  El estudiante sólo podrá seleccionar 3 materias.
6.  Hay 5 profesores que dictan 2 materias cada uno.
7.  El estudiante no podrá tener clases con el mismo profesor.
8.  Cada estudiante puede ver en línea los registros de otros estudiantes.
9.  El estudiante podrá ver sólo el nombre de los alumnos con quienes compartirá cada clase.
---

## ✨ Características Principales

* **Autenticación de Usuarios**: Sistema de login seguro basado en **Tokens JWT**.
* **Registro de Cuentas**: Flujo de registro de nuevos estudiantes.
* **Inscripción de Materias**: Un formulario reactivo que permite al estudiante autenticado inscribir materias.
* **Validaciones de Negocio en Tiempo Real**:
    * El estudiante solo puede seleccionar un **máximo de 3 materias**.
    * El estudiante **no puede inscribir materias** con el mismo profesor.
* **Consulta de Datos**:
    * Un estudiante puede ver una lista de **sus compañeros de clase**.
    * Un catálogo público de las materias disponibles con sus respectivos profesores.

---

## 🚀 Arquitectura y Puntos a Destacar

Esta aplicación no es solo un CRUD, sino una demostración de prácticas de desarrollo modernas.

### Backend (.NET 9)

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

* Se debe crear la base de datos y las tablas relacionadas mediante el script que se encuentra en este mismo repositorio:
  
    * **Ubicación del Script**: El script se encuentra dentro de la carpeta del proyecto `StudentRegistration`.
    * **Nombre del Script**: `Create_DataBase_And_Tables_DanielRamirez_PruebaInterRapidisimo.sql`
---

## ⚙️ Ejecución del Proyecto

Sigue estos pasos en orden para levantar la aplicación.

**1. Levantar el Backend (.NET API)**

* Navega a la carpeta del proyecto API desde una terminal:
    ```bash
    cd StudentRegistration.Presentation.API
    ```
* Ejecuta la API con el siguiente comando:
    ```bash
    dotnet run
    ```
* La API se iniciará. **Observa la URL en la que está escuchando** en la terminal (por ejemplo, `Now listening on: https://localhost:7123`). Anota esta URL.

---

**2. Configurar y Levantar el Frontend (Angular)**

* Abre **otra terminal** y navega a la carpeta del proyecto de Angular:
    ```bash
    cd StudentRegistration.UI
    ```
* **Paso de Configuración Crucial**:
    * Abre el archivo `src/environments/environment.ts`.
    * Busca la propiedad `apiUrl` y **asegúrate de que su valor coincida exactamente con la URL del backend** que anotaste en el paso anterior. Por ejemplo:
        `apiUrl: 'https://localhost:7123/api'`
* Instala las dependencias (solo la primera vez):
    ```bash
    npm install
    ```
* Una vez configurado, levanta el servidor de desarrollo:
    ```bash
    ng serve
    ```

---

**3. Acceder a la Aplicación**

* Abre tu navegador web y ve a `http://localhost:4200`.
* Inicia sesión con la cuenta de prueba para empezar a usar la aplicación.
    * **Usuario**: `test@test.com`
    * **Contraseña**: `123456`

---
