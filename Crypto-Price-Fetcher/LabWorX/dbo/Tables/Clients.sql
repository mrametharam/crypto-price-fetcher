CREATE TABLE [dbo].[Clients] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (50) NULL,
    [LastName]    NVARCHAR (50) NULL,
    [Email]       NVARCHAR (50) NULL,
    [DateOfBirth] DATE          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

