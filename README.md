# 🚀 Proyecto de Registro Estudiantil - Prueba Técnica

## Resumen de la Aplicación

Aplicación Full-Stack de tipo Cliente-Servidor desarrollada con **.NET 9** y **Angular 17** que implementa un sistema de registro académico. La plataforma permite a los usuarios registrarse, iniciar sesión e inscribir materias bajo un conjunto de reglas de negocio específicas, como un límite de créditos y restricciones de profesorado.

La seguridad se gestiona mediante autenticación basada en **Tokens JWT** y **Refresh Tokens**, protegiendo las rutas y la comunicación entre el cliente y el servidor.

---

## 📋 Requerimientos del Ejercicio

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

* **Autenticación Segura y Persistente**: Sistema de login robusto basado en **Tokens JWT** para la autorización de peticiones y **Refresh Tokens** para mantener la sesión del usuario activa de forma segura.
* **Registro de Cuentas**: Flujo de registro de nuevos estudiantes.
* **Inscripción de Materias**: Un formulario reactivo que permite al estudiante autenticado inscribir materias.
* **Validaciones de Negocio en Tiempo Real**:
    * El estudiante solo puede seleccionar un **máximo de 3 materias**.
    * El estudiante **no puede inscribir materias** con el mismo profesor.
* **Consulta de Datos**:
    * Un estudiante puede ver una lista de **sus compañeros de clase**.
    * Un catálogo público de las materias disponibles con sus respectivos profesores.

---

## 🏛️ Arquitectura y Puntos a Destacar

Esta aplicación no es solo un CRUD, sino una demostración de prácticas de desarrollo modernas.

### Backend (.NET 9)

* **Arquitectura Limpia (Clean Architecture)**: Se separaron las responsabilidades del sistema en capas (`Domain`, `Application`, `Infrastructure`, `Presentation`).
* **Seguridad Robusta**:
    * **Hashing de Contraseñas**: Las credenciales de los usuarios nunca se guardan en texto plano. Se almacenan como **hashes seguros** para garantizar su integridad.
    * **Autenticación por Tokens**: Se utiliza un esquema de **Tokens JWT** para autorizar cada petición al API y **Refresh Tokens** para renovar sesiones caducadas sin requerir nuevas credenciales.
      
* **Contratos de Datos con DTOs y AutoMapper**: Para garantizar la seguridad y un diseño desacoplado, no se exponen las entidades de la base de datos directamente. Se utiliza el patrón DTO (Data Transfer Object) para crear "contratos" de API explícitos, enviando solo la información necesaria. El mapeo entre entidades y DTOs se gestiona de forma eficiente y automática con **AutoMapper**.
* **Patrón CQRS con MediatR**: La lógica de negocio está aislada en `Commands` y `Queries`.
* **Unit of Work y Repositorio**: Se abstrajo el acceso a datos para centralizar la gestión de la base de datos.
* **Manejo de Errores Global**: Un `Middleware` personalizado captura excepciones no controladas.
* **Data Seeding con Bogus**: El API puebla automáticamente la base de datos con datos de prueba realistas.

### Frontend (Angular 17)

* **Arquitectura Standalone**: Se utilizó la arquitectura de componentes `standalone` más reciente.
* **Formularios Reactivos**: Manejo robusto del estado del formulario y validaciones.
* **Autenticación Segura**: Uso de **Route Guards** y un **HTTP Interceptor** para gestionar el ciclo de vida de los tokens (JWT y Refresh) y proteger las rutas.
* **Alias de Rutas (`tsconfig.json`)**: Se configuraron alias para importaciones limpias (`@features`, `@shared`).
* **Interfaz Atractiva**: Se utilizó **Angular Material** para construir una interfaz de usuario intuitiva.

---

## 🗃️ Configuración de la Base de Datos

**1. Cadena de Conexión**

El API está configurado por defecto para conectarse a un servidor SQL Server local. Verifica que esta configuración en el archivo `StudentRegistration.Presentation.API\appsettings.json` coincida con tu entorno.

* **Servidor**: `localhost\sqlexpress`
* **Nombre de la Base de Datos**: `DanielRamirez_PruebaInterRapidisimo`

**2. Creación de la Base de Datos**

Tienes dos opciones para crear y poblar la base de datos:

* **Opción A (Automática - Recomendada)**:
  
    * Simplemente **ejecuta el proyecto API (`dotnet run`) en modo de Desarrollo**.
    * El sistema está configurado para detectar si la base de datos no existe y la **creará y poblará automáticamente con los siguientes datos iniciales**:
        * Los 5 profesores.
        * Las 10 materias, asignadas a los profesores.
        * Una cuenta de usuario por defecto para pruebas (`test@test.com`).

* **Opción B (Manual)**:
  
    * Si prefieres crear la estructura manualmente, puedes ejecutar el script `Create_DataBase_And_Tables_DanielRamirez_PruebaInterRapidisimo.sql` que se encuentra en la carpeta del proyecto `StudentRegistration`.

---

## ⚙️ Ejecución del Proyecto

Sigue estos pasos en orden para levantar la aplicación.

**1. Clonar el Repositorio**

```bash
git clone https://github.com/dannielrammirez/StudentRegistration.git
```

**2. Levantar el Backend (.NET API)**

* Navega a la carpeta del proyecto API desde una terminal.
  
    ```bash
    cd StudentRegistration/StudentRegistration.Presentation.API
    ```
    
* Ejecuta el comando para iniciar el API.
  
    ```bash
    dotnet run
    ```
    
* **Observa la URL** en la que está escuchando la API. Por ejemplo: `https://localhost:7123`.

**3. Configurar y Levantar el Frontend (Angular)**

* Abre **otra terminal** y navega a la carpeta del proyecto Angular.
  
    ```bash
    cd StudentRegistration/StudentRegistration.UI
    ```
    
* **Paso de Configuración Crucial**:
    * Abre el archivo ubicado en la ruta `StudentRegistration.UI/src/app/environments/environment.ts`.
    * Busca la propiedad `apiUrl` y **asegúrate de que su valor coincida exactamente con la URL del backend** del paso anterior.
      
* Instala las dependencias (solo la primera vez).
  
    ```bash
    npm install
    ```
    
* Levanta el servidor de desarrollo.
  
    ```bash
    ng serve
    ```
    
**4. Acceder a la Aplicación**

* Abre tu navegador y ve a `http://localhost:4200`.
* Para usar la aplicación, inicia sesión creando una cuenta o utiliza las credenciales por defecto:
    * **Usuario**: `test@test.com`
    * **Contraseña**: `123456`
 
 ---
