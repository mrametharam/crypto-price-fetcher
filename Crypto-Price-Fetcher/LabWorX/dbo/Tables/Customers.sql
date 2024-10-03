CREATE TABLE [dbo].[Customers] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   VARCHAR (50)  NOT NULL,
    [LastName]    VARCHAR (50)  NOT NULL,
    [Email]       VARCHAR (100) NOT NULL,
    [DateOfBirth] DATE          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

