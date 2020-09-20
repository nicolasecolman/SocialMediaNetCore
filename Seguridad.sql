USE [SocialMedia]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE Seguridad 
(
	IdSeguridad int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Usuario Varchar(50) NOT NULL,
	NombreUsuario Varchar(100) NOT NULL,
	Contrasena Varchar(200) NOT NULL,
	Rol Varchar(15) NOT NULL
)
GO
