-- Step 1: Create a new database
CREATE DATABASE [db_todolist];
GO

-- Step 2: Create a SQL Server login with a password
CREATE LOGIN William WITH PASSWORD = 'test123@';
GO

-- Step 3: Use the database and create a user mapped to the login
USE [db_todolist];
GO
CREATE USER William FOR LOGIN William;
GO

-- Step 4: Grant necessary permissions to the user (e.g., db_owner role)
ALTER ROLE db_owner ADD MEMBER William;
GO


USE [db_todolist]
GO
/****** Object:  StoredProcedure [dbo].[sp_register_user]    Script Date: 27/07/2025 12:14:07 ******/
DROP PROCEDURE [dbo].[sp_register_user]
GO
/****** Object:  StoredProcedure [dbo].[sp_login]    Script Date: 27/07/2025 12:14:07 ******/
DROP PROCEDURE [dbo].[sp_login]
GO
ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK__UserRoles__UserI__1BC821DD]
GO
ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK__UserRoles__RoleI__07C12930]
GO
ALTER TABLE [dbo].[ToDoTasks] DROP CONSTRAINT [FK__Tasks__ListId__2BFE89A6]
GO
ALTER TABLE [dbo].[ToDoLists] DROP CONSTRAINT [FK__Lists__UserId__2B0A656D]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF__Users__CreatedAt__286302EC]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/07/2025 12:14:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 27/07/2025 12:14:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
DROP TABLE [dbo].[UserRoles]
GO
/****** Object:  Table [dbo].[ToDoTasks]    Script Date: 27/07/2025 12:14:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ToDoTasks]') AND type in (N'U'))
DROP TABLE [dbo].[ToDoTasks]
GO
/****** Object:  Table [dbo].[ToDoLists]    Script Date: 27/07/2025 12:14:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ToDoLists]') AND type in (N'U'))
DROP TABLE [dbo].[ToDoLists]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 27/07/2025 12:14:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
DROP TABLE [dbo].[Roles]
GO

/****** Object:  UserDefinedFunction [dbo].[GetPepper]    Script Date: 27/07/2025 12:14:07 ******/
DROP FUNCTION [dbo].[GetPepper]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPepper]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetPepper]
(	
)
RETURNS NVARCHAR(100) 
AS
BEGIN
	RETURN 'ITS-WILLIAM-PEPPER-5be28990-ee2f-4a04-aa72-3b39f4e0c1c3'
END

GO

/****** Object:  Table [dbo].[Roles]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_Roles_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDoLists]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDoLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK__Lists__3214EC07F52BEF00] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDoTasks]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDoTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DueDate] [datetime] NULL,
	[Status] [nvarchar](20) NOT NULL,
	[ListId] [int] NOT NULL,

 CONSTRAINT UQ_Users_Email UNIQUE (Email),
 CONSTRAINT [PK__Tasks__3214EC07588F060B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedById] [int] NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[CreateRoleDate] [datetime] NULL,
 CONSTRAINT [PK_UserRoles_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PasswordHash] [varbinary](64) NOT NULL,
	[BirthDate] [datetime] NULL,
	[Salt] [varchar](50) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK__tmp_ms_x__3214EC073AAB4F5D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ToDoLists]  WITH CHECK ADD  CONSTRAINT [FK__Lists__UserId__2B0A656D] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ToDoLists] CHECK CONSTRAINT [FK__Lists__UserId__2B0A656D]
GO
ALTER TABLE [dbo].[ToDoTasks]  WITH CHECK ADD  CONSTRAINT [FK__Tasks__ListId__2BFE89A6] FOREIGN KEY([ListId])
REFERENCES [dbo].[ToDoLists] ([Id])
GO
ALTER TABLE [dbo].[ToDoTasks] CHECK CONSTRAINT [FK__Tasks__ListId__2BFE89A6]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK__UserRoles__RoleI__07C12930] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK__UserRoles__RoleI__07C12930]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK__UserRoles__UserI__1BC821DD] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK__UserRoles__UserI__1BC821DD]
GO
/****** Object:  StoredProcedure [dbo].[sp_login]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_login]
    @LoginInput NVARCHAR(100),  -- Username o Email
    @Password NVARCHAR(100)     -- Plaintext inserita dall'utente
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT;
    DECLARE @Salt NVARCHAR(36);
    DECLARE @PasswordHash VARBINARY(64);

    -- Cerca utente per Username o Email
    SELECT TOP 1 
        @UserId = Id,
        @Salt = Salt
    FROM Users
    WHERE UserName = @LoginInput OR Email = @LoginInput;

    -- Se non trovato, esci subito
    IF @UserId IS NULL
    BEGIN
        RETURN;
    END

    -- Ottieni il pepper dalla funzione
    DECLARE @Pepper NVARCHAR(100) = dbo.GetPepper();

    -- Calcola hash della password inserita
    SET @PasswordHash = HASHBYTES(
        'SHA2_512',
        CONCAT(@Password , @Salt , @Pepper)
    );

    -- Confronta hash e restituisci dati se valido
    SELECT 
        Id,
        FirstName,
        LastName,
        Email,
        BirthDate,
        CreatedAt
    FROM Users
    WHERE Id = @UserId AND PasswordHash = @PasswordHash;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_register_user]    Script Date: 27/07/2025 12:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_register_user]
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @UserName NVARCHAR(50),
    @BirthDate DATETIME = NULL,
    @Email NVARCHAR(100),
    @Password NVARCHAR(100), -- Plaintext
    @RoleId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Salt NVARCHAR(50) = NEWID();

        -- Ottieni il pepper da funzione esterna
        DECLARE @Pepper NVARCHAR(100) = [dbo].GetPepper();

        -- Hash password + salt + pepper
        DECLARE @PasswordHash VARBINARY(64);
        SET @PasswordHash = HASHBYTES(
            'SHA2_512',
            CONCAT(@Password , @Salt , @Pepper)
        );

        -- Inserisci utente
        INSERT INTO Users (
            FirstName,
            LastName,
            UserName,
            BirthDate,
            Email,
            PasswordHash,
            Salt,
            CreatedAt
        )
        VALUES (
            @FirstName,
            @LastName,
            @UserName,
            @BirthDate,
            @Email,
            @PasswordHash,
            @Salt,
            GETDATE()
        );

        DECLARE @NewUserId INT = SCOPE_IDENTITY();

        -- Controlla RoleId o usa "Default"
        DECLARE @FinalRoleId INT;
        IF EXISTS (SELECT 1 FROM Roles WHERE Id = @RoleId)
            SET @FinalRoleId = @RoleId;
        ELSE
            SELECT @FinalRoleId = Id FROM Roles WHERE RoleName = 'Default';

        -- Inserisci relazione utente-ruolo
        INSERT INTO UserRoles (
            UserId,
            RoleId,
            CreateRoleDate
        )
        VALUES (
            @NewUserId,
            @FinalRoleId,
            GETDATE()
        );

        COMMIT;

        -- 🔁 Ritorna l'ID dell'utente
        SELECT @NewUserId;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END;
GO
