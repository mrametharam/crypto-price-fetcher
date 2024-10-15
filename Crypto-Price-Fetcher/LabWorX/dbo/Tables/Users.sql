CREATE TABLE [dbo].[Users]
(
	[Id]            INT           NOT NULL  PRIMARY KEY IDENTITY(1,1),
    [Username]      NVARCHAR(50)  NOT NULL,
    [PasswordHash]  NVARCHAR(255) NOT NULL
)
