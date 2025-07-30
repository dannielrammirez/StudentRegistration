-- =================================================================================
-- Script: Creación de Base de Datos y Esquema para Prueba Inter Rapidísimo
-- Autor: Daniel Ramirez
-- Descripción: Crea la base de datos y todas las tablas necesarias para la
--              aplicación de registro de estudiantes. Es transaccional y seguro.
-- =================================================================================

-- Nombre de la base de datos a utilizar
DECLARE @DBName NVARCHAR(128) = 'DanielRamirez_PruebaInterRapidisimo';

-- Creación de la Base de Datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @DBName)
BEGIN
    PRINT 'Creando la base de datos ' + @DBName + '...';
    EXEC('CREATE DATABASE ' + @DBName);
    PRINT 'Base de datos ' + @DBName + ' creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos ' + @DBName + ' ya existe.';
END
GO

USE DanielRamirez_PruebaInterRapidisimo;
GO

-- =================================================================================
-- INICIO: Creación de Tablas dentro de una Transacción
-- =================================================================================
BEGIN TRANSACTION;
PRINT 'Iniciando transacción para la creación del esquema...';

BEGIN TRY

    -- Tabla: Students
    PRINT 'Verificando la tabla [dbo].[Students]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[Students]...';
        CREATE TABLE [dbo].[Students](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [FirstName] [nvarchar](100) NOT NULL,
            [LastName] [nvarchar](100) NOT NULL,
            [Email] [nvarchar](255) NOT NULL,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL
        );
        PRINT 'Tabla [dbo].[Students] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[Students] ya existe.';
    END

    -- Tabla: Accounts
    PRINT 'Verificando la tabla [dbo].[Accounts]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[Accounts]...';
        CREATE TABLE [dbo].[Accounts](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [Username] [nvarchar](100) NOT NULL,
            [Email] [nvarchar](255) NOT NULL,
            [PasswordHash] [varbinary](max) NOT NULL,
            [PasswordSalt] [varbinary](max) NOT NULL,
            [IdReferencia] [uniqueidentifier] NOT NULL,
            [TipoCuenta] [int] NOT NULL,
            [IdEstado] [int] NOT NULL DEFAULT 1,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL,
            CONSTRAINT [UQ_Accounts_Email] UNIQUE ([Email]),
            CONSTRAINT [UQ_Accounts_Username] UNIQUE ([Username])
        );
        PRINT 'Tabla [dbo].[Accounts] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[Accounts] ya existe.';
    END

    -- Tabla: Professors
    PRINT 'Verificando la tabla [dbo].[Professors]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Professors]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[Professors]...';
        CREATE TABLE [dbo].[Professors](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [FirstName] [nvarchar](100) NOT NULL,
            [LastName] [nvarchar](100) NOT NULL,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL
        );
        PRINT 'Tabla [dbo].[Professors] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[Professors] ya existe.';
    END

    -- Tabla: Courses
    PRINT 'Verificando la tabla [dbo].[Courses]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[Courses]...';
        CREATE TABLE [dbo].[Courses](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [Name] [nvarchar](150) NOT NULL,
            [Credits] [int] NOT NULL,
            [ProfessorId] [uniqueidentifier] NOT NULL,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL,
            CONSTRAINT [FK_Courses_Professors] FOREIGN KEY ([ProfessorId]) REFERENCES [dbo].[Professors] ([Id])
        );
        PRINT 'Tabla [dbo].[Courses] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[Courses] ya existe.';
    END

    -- Tabla: Enrollments
    PRINT 'Verificando la tabla [dbo].[Enrollments]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[Enrollments]...';
        CREATE TABLE [dbo].[Enrollments](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [StudentId] [uniqueidentifier] NOT NULL,
            [CourseId] [uniqueidentifier] NOT NULL,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL,
            CONSTRAINT [FK_Enrollments_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students] ([Id]),
            CONSTRAINT [FK_Enrollments_Courses] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Courses] ([Id])
        );
        PRINT 'Tabla [dbo].[Enrollments] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[Enrollments] ya existe.';
    END

    -- Tabla: RefreshTokens
    PRINT 'Verificando la tabla [dbo].[RefreshTokens]...';
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
    BEGIN
        PRINT 'Creando la tabla [dbo].[RefreshTokens]...';
        CREATE TABLE [dbo].[RefreshTokens](
            [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
            [Token] [nvarchar](max) NOT NULL,
            [Expires] [datetime2](7) NOT NULL,
            [Revoked] [datetime2](7) NULL,
            [IsRevoked] [bit] NOT NULL DEFAULT 0,
            [ReplacedByToken] [nvarchar](max) NULL,
            [IdCuenta] [uniqueidentifier] NOT NULL,
            [IsActive] [bit] NOT NULL DEFAULT 1,
            [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
            [UpdatedAt] [datetime2](7) NULL,
            CONSTRAINT [FK_RefreshTokens_Accounts] FOREIGN KEY ([IdCuenta]) REFERENCES [dbo].[Accounts] ([Id])
        );
        PRINT 'Tabla [dbo].[RefreshTokens] creada.';
    END
    ELSE
    BEGIN
        PRINT 'La tabla [dbo].[RefreshTokens] ya existe.';
    END

    COMMIT TRANSACTION;
    PRINT 'Transacción completada. El esquema de la base de datos se ha creado/verificado exitosamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT 'Ocurrió un error. La transacción ha sido revertida.';
    
    -- Lanza el error original para obtener detalles
    THROW;
END CATCH
GO
-- =================================================================================
-- FIN: Creación de Tablas
-- =================================================================================